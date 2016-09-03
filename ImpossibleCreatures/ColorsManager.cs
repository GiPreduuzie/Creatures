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
        public Dictionary<int, Color> UsedColors = new Dictionary<int, Color>();
        private Random _random = new Random();

        public ColorsPair GetColors(VisualizationType visualizationType, int energy, int food, int parentMark)
        {
            var fillColor = new Color();
            var strokeColor = new Color();

            switch (visualizationType)
            {
                case VisualizationType.FillBlackStrokeCanEat:
                    if (energy != 0)
                    {
                        fillColor = Colors.Black;
                        strokeColor = food != 0 ? Colors.YellowGreen : Colors.OrangeRed;
                    }
                    else
                    {
                        fillColor = food != 0 ? Color.FromArgb(50, 154, 205, 50) : Colors.White;
                        strokeColor = fillColor;
                    }

                    break;
                case VisualizationType.FillEnergyStrokeCanEat:
                    if (energy != 0)
                    {
                        fillColor = GetCreatureFillEnergyColor(energy);
                        strokeColor = food != 0 ? Colors.YellowGreen : Colors.OrangeRed;
                    }
                    else
                    {
                        fillColor = food != 0 ? Color.FromArgb(50, 154, 205, 50) : Colors.White;
                        strokeColor = fillColor;
                    }
                    break;
                case VisualizationType.FillNation:
                    if (parentMark != -1)
                    {
                        if (!UsedColors.ContainsKey(parentMark))
                        {
                            UsedColors[parentMark] = Color.FromArgb(255, (byte)_random.Next(255), (byte)_random.Next(255), (byte)_random.Next(255));
                        }
                        fillColor = UsedColors[parentMark];
                        strokeColor = fillColor;
                    }
                    else
                    {
                        fillColor = food != 0 ? Color.FromArgb(50, 154, 205, 50) : Colors.White;
                        strokeColor = fillColor;
                    }
                    break;
            }

            return new ColorsPair(fillColor, strokeColor);
        }

        private Color GetCreatureFillEnergyColor(int currentEnergy)
        {
            double multiplier = (double)255 / (CreatureConstants.StartEnergyPoints * 5);

            int energy = (int)(currentEnergy <= 0 ? 0 : currentEnergy * multiplier);
            if (energy > 255)
                energy = 255;

            var val = (byte)(255 - energy * multiplier);
            return Color.FromArgb(255, val, val, val);
        }
    }
}
