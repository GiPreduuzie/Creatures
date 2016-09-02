using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CellsAutomate;
using CellsAutomate.Constants;
using Color = System.Windows.Media.Color;
using Matrix = CellsAutomate.Matrix;

namespace ImpossibleCreatures
{
    public partial class MainWindow : Window
    {
        private const int BitmapSize = 1000;

        private void PrintBitmap(int length, Matrix matrix)
        {
            _paintTimer.Start();

            var writeableBmp = BitmapFactory.New(BitmapSize, BitmapSize);
            MainImage.Source = writeableBmp;
            using (writeableBmp.GetBitmapContext())
            {
                writeableBmp.Clear(Colors.White);

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

                        DrawRectangle(writeableBmp, length, i, j, fillColor, strokeColor);
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