using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using Point = System.Drawing.Point;

namespace ImpossibleCreatures
{
    public partial class MainWindow : Window
    {
        private ColorsManager _colorsManager = new ColorsManager();

        private const int BitmapSize = 1000;

        private void PrintBitmap()
        {
            _paintTimer.Start();

            var size = _dependenciesResolver.GetMatrixSize();

            var writeableBmp = BitmapFactory.New(BitmapSize, BitmapSize);
            MainImage.Source = writeableBmp;
            using (writeableBmp.GetBitmapContext())
            {
                writeableBmp.Clear(Colors.White);

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        var creature = _matrix.Creatures[i, j];
                        var energy = creature != null ? creature.EnergyPoints : 0;
                        var food = _matrix.EatMatrix.HasOneBite(new Point(i, j)) ? 1 : 0;
                        var parent = creature != null ? creature.ParentMark : -1;

                        var colors = _colorsManager.GetColors(_visualizationType, energy, food, parent);

                        DrawRectangle(writeableBmp, size, i, j, colors.FillColor, colors.StrokeColor);
                    }
                }
            }
                
            _paintTimer.Stop();
        }

        private static void DrawRectangle(WriteableBitmap writeableBmp, int matrixSize, int i, int j, Color fillColor, Color strokeColor)
        {
            int shapeSize = BitmapSize/matrixSize;
            int borderSize = 2;

            // outer
            writeableBmp.FillRectangle(
                i*shapeSize,
                j*shapeSize,
                (i + 1)*shapeSize - 1,
                (j + 1)*shapeSize - 1,
                strokeColor);

            // inner
            writeableBmp.FillRectangle(
                i*shapeSize + borderSize,
                j*shapeSize + borderSize,
                (i + 1)*shapeSize - 1 - borderSize,
                (j + 1)*shapeSize - 1 - borderSize,
                fillColor);
        }
    }
}