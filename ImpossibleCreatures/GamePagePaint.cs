using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
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
    }
}