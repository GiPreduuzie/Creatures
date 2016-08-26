﻿using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ImpossibleCreatures
{
    public partial class MainWindow : Window
    {
        private SolidColorBrush emptyColor = new SolidColorBrush(Colors.Black);

        private void PaintSquare(int x, int y, Color color, Rectangle[,] squares)
        {
            squares[x, y].Fill = new SolidColorBrush(color);
            squares[x, y].Stroke = new SolidColorBrush(Colors.Black);
            squares[x, y].StrokeThickness = 3;
        }

        private void CleanSquare(int column, int row, Rectangle[,] doubleElements)
        {
            doubleElements[column, row].Fill = emptyColor;
            doubleElements[column, row].StrokeThickness = 0;
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
    }
}