using System;
using System.Collections.Generic;
using System.Windows.Media;
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

        public ColorsPair GetColors(VisualizationType visualizationType, int energy, int maxEnergy, int food, int maxFood, int parentMark)
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
                        fillColor = food != 0 ? Color.FromArgb(128, 154, 205, 50) : Colors.White;
                        strokeColor = fillColor;
                    }
                    else
                    {
                        fillColor = GetEnergyColor(energy, maxEnergy);
                        strokeColor = fillColor;
                    }
                    break;
                case VisualizationType.Food:
                    if (food == 0)
                    {
                        fillColor = energy == 0 ? Colors.White : Color.FromArgb(128, 128, 128, 128);
                        strokeColor = fillColor;
                    }
                    else
                    {
                        fillColor = GetFoodColor(food, maxFood);
                        strokeColor = fillColor;
                    }
                    break;
                case VisualizationType.Experimantal:

                    break;

            }

            return new ColorsPair(fillColor, strokeColor);
        }

        private Color GetFoodColor(int food, int maxFood)
        {
            var r = (byte)(0);
            var g = (byte)(255 * ((food * 1.0) / maxFood));
            var b = (byte)(0);

            var color = Color.FromArgb(255, r, g, b);
            return color;
        }

        private Color GetEnergyColor(int energy, int maxEnergy)
        {
            var r = (byte)(255 * ((energy * 1.0) / maxEnergy));
            var g = (byte)(0);
            var b = (byte)(0);

            var color = Color.FromArgb(255, r, g, b);
            return color;
        }
    }
}
