using System;
using System.Collections.Generic;
using System.Windows.Media;
using CellsAutomate.Constants;
using Color = System.Windows.Media.Color;

namespace ImpossibleCreatures
{
    internal class ColorsPair
    {
        public Color FillColor { get; private set; }
        public Color StrokeColor { get; private set; }

        public ColorsPair(Color fillColor, Color strokeColor)
        {
            StrokeColor = strokeColor;
            FillColor = fillColor;
        }
    }

    internal class ColorsManager
    {
        private Dictionary<int, Color> _usedColors;
        private readonly Random _random;

        public ColorsManager()
        {
            _usedColors = new Dictionary<int, Color>();
            _random = new Random();
        }

        public void Reset()
        {
            _usedColors = new Dictionary<int, Color>();
        }

        public ColorsPair GetColors(VisualizationType visualizationType, int energy, int food, int parentMark)
        {
            var fillColor = new Color();
            var strokeColor = new Color();

            switch (visualizationType)
            {
                case VisualizationType.CanEat:
                    if (energy == 0)
                    {
                        fillColor = food != 0 ? Color.FromArgb(50, 154, 205, 50) : Colors.White;
                        strokeColor = fillColor;
                    }
                    else
                    {
                        fillColor = food != 0 ? Colors.YellowGreen : Colors.OrangeRed;
                        strokeColor = fillColor;
                    }
                    break;
                case VisualizationType.Nation:
                    if (energy == 0)
                    {
                        fillColor = Colors.White;
                        strokeColor = fillColor;
                    }
                    else
                    {
                        if (!_usedColors.ContainsKey(parentMark))
                        {
                            _usedColors[parentMark] = Color.FromArgb(255, (byte)_random.Next(255), (byte)_random.Next(255), (byte)_random.Next(255));
                        }
                        fillColor = _usedColors[parentMark];
                        strokeColor = fillColor;
                    }
                    break;
                case VisualizationType.Energy:
                    if (energy == 0)
                    {
                        fillColor = food != 0 ? Color.FromArgb(50, 154, 205, 50) : Colors.White;
                        strokeColor = fillColor;
                    }
                    else
                    {
                        fillColor = GetEnergyColor(energy);
                        strokeColor = fillColor;
                    }
                    break;
                case VisualizationType.Experimantal:

                    break;

            }

            return new ColorsPair(fillColor, strokeColor);
        }

        private Color GetEnergyColor(int energy)
        {
            if (energy >= 255)
            {
                energy = 255;
            }

            var r = (byte)(128 + 127 * ((energy * 1.0) / 255));
            var g = (byte)(128 - 128 * ((energy * 1.0) / 255));
            var b = (byte)(128 - 128 * ((energy * 1.0) / 255));

            var color = Color.FromArgb(255, r, g, b);
            return color;
        }
    }
}
