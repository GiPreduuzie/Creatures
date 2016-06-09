using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace CellsAutomate
{
    class Program
    {
        private static void Main(string[] args)
        {
            var length = 500;

            var matrix = new Matrix();
            matrix.Cells = new SimpleCreature[length, length];
            matrix.N = length;
            matrix.M = length;

            matrix.FillStartMatrixRandomly();
            Print(0, length, matrix);

            Console.WriteLine("0:{0}", matrix.AliveCount);

            for (int i = 0; i < 1000; i++)
            {
                matrix.MakeTurn();
                Print(i + 1, length, matrix);
                Console.WriteLine("{0}:{1}", i + 1, matrix.AliveCount);
            }
        }

        private static void Print(int id, int length, Matrix matrix)
        {
            if (id % 10 != 0) return;

            var bitmap = new Bitmap(length, length);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    bitmap.SetPixel(i, j, matrix.Cells[i, j] == null ? Color.White : Color.Red);
                }
            }

            bitmap.Save($@"C:\temp\bitmaps\{id}.jpg", ImageFormat.Jpeg);
        }
    }
}