using System.Windows;
using System.Windows.Input;
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
            _paintTimer.Reset();
            _paintTimer.Start();
            var matrix = _matrix.Value;
            var maxEnergy = matrix.GetMaxEnergy();
            var maxFood = matrix.EatMatrix.GetMaxLevelOfFood();
            var maxAge = matrix.GetMaxAge();

            var writeableBmp = BitmapFactory.New(BitmapSize, BitmapSize);
            MainImage.Source = writeableBmp;
            using (writeableBmp.GetBitmapContext())
            {
                writeableBmp.Clear(Colors.White);

                for (int i = 0; i < matrix.Height; i++)
                {
                    for (int j = 0; j < matrix.Length; j++)
                    {
                        var creature = matrix.Creatures[i, j];
                        var energy = creature?.EnergyPoints ?? 0;
                        var food = matrix.EatMatrix.GetLevelOfFood(new Point(i, j));
                        var parent = creature?.ParentMark ?? -1;
                        var age = creature?.Age ?? 0;

                        var colors = _colorsManager.GetColors(_visualizationType, energy, maxEnergy, age, maxAge, food, maxFood, parent);

                        DrawRectangle(writeableBmp, matrix.Length, i, j, colors.FillColor, colors.StrokeColor);
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

            if (fillColor != strokeColor)
            {
                // inner
                writeableBmp.FillRectangle(
                    i*shapeSize + borderSize,
                    j*shapeSize + borderSize,
                    (i + 1)*shapeSize - 1 - borderSize,
                    (j + 1)*shapeSize - 1 - borderSize,
                    fillColor);
            }
        }

        private void MainImage_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double cellSize = MainImage.ActualHeight/_matrix.Value.Length;

            var position = e.GetPosition(MainImage);

            int cellX = (int)(position.X/cellSize);
            int cellY = (int)(position.Y / cellSize);

            var creature = _matrix.Value.Creatures[cellX, cellY];

            if (creature != null)
            {
                var creatureInfo = new CreatureInfo(creature);
                creatureInfo.Show();
            }
        }
    }
}