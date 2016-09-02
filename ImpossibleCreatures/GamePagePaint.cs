﻿using System.Drawing;
using System.Windows;
using System.Windows.Media;
using CellsAutomate;
using CellsAutomate.Constants;
using Color = System.Windows.Media.Color;
using Matrix = CellsAutomate.Matrix;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace ImpossibleCreatures
{
    public partial class MainWindow : Window
    {
        private SolidColorBrush emptyColor = new SolidColorBrush(Colors.Black);

        private void PaintSquareFill(int x, int y, Color color, Rectangle[,] squares)
        {
            var brush = squares[x, y].Fill as SolidColorBrush;
            if(brush != null && brush.Color == color)
                return;

            squares[x, y].Fill = new SolidColorBrush(color);
        }

        private void PaintSquareStroke(int x, int y, Color color, Rectangle[,] squares)
        {
            var brush = squares[x, y].Stroke as SolidColorBrush;
            if (brush != null && brush.Color == color)
                return;

            squares[x, y].Stroke = new SolidColorBrush(color);
        }

        private void CleanSquare(int column, int row, Rectangle[,] doubleElements)
        {
            doubleElements[column, row].Fill = emptyColor;
            doubleElements[column, row].StrokeThickness = 3;
        }

        private void CleanGrid(Rectangle[,] doubleElements)
        {
            for (int i = 0; i < doubleElements.GetLength(0); i++)
            {
                for (int j = 0; j < doubleElements.GetLength(1); j++)
                {
                    CleanSquare(i, j, doubleElements);
                }
            }
        }

        private void Print(int id, int length, Matrix matrix)
        {
            _paintTimer.Start();

            using (Dispatcher.DisableProcessing())
            {
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        var isThereAreFood = matrix.EatMatrix.HasOneBite(new System.Drawing.Point(i, j));
                        var creature = matrix.Creatures[i, j];
                        var isThereAreCreature = creature != null;
                        Color strokeColor;
                        Color fillColor;

                        if (isThereAreCreature)
                        {
                            fillColor = GetCreatureFillColor(creature);
                            strokeColor = isThereAreFood ? Colors.YellowGreen : Colors.OrangeRed;
                        }
                        else
                        {
                            if (isThereAreFood)
                            {
                                fillColor = Color.FromArgb(50, 154, 205, 50);
                                strokeColor = fillColor;
                            }
                            else
                            {
                                fillColor = Colors.White;
                                strokeColor = fillColor;
                            }
                        }

                        PaintSquareStroke(i, j, strokeColor, _squares);
                        PaintSquareFill(i, j, fillColor, _squares);
                    }
                }
            }
            _paintTimer.Stop();
        }

        private Color GetCreatureFillColor(Membrane creature)
        {
            if (!MenuItemEnergy.IsChecked)
                return Colors.Black;

            double multiplier = (double) 255/(CreatureConstants.StartEnergyPoints*5);

            int energy = (int) (creature.EnergyPoints < 0 ? 0 : creature.EnergyPoints*multiplier);
            if (energy > 255)
                energy = 255;

            var val = (byte) (255 - energy*multiplier);
            return Color.FromArgb(255, val, val, val);
        }
    }
}