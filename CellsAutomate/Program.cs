﻿using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace CellsAutomate
{
    class Program
    {
        private static void Main(string[] args)
        {
            var length = 10;

            var matrix = new Matrix();
            matrix.Cells = new SimpleCreature[length, length];
            matrix.N = length;
            matrix.M = length;

            matrix.FillStartMatrixRandomly();
            Print(0, length, matrix, new bool[length, length]);

            Console.WriteLine("0:{0}", matrix.AliveCount);

            for (int i = 0; i < 100; i++)
            {
                var eat = matrix.MakeTurn();
                Print(i + 1, length, matrix, eat);
                Console.WriteLine("{0}:{1}", i + 1, matrix.AliveCount);
            }
        }

        private static void Print(int id, int length, Matrix matrix, bool[,] eat)
        {
            if (id % 10 != 0) return;

            var bitmap = new Bitmap(length*2, length);

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    bitmap.SetPixel(i, j, eat[i, j] ? Color.White : Color.Green);
                }
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    var x = i + length;
                    var y = j;

                    bitmap.SetPixel(x, y, matrix.Cells[i, j] == null ? Color.White : Color.Red);

                    // bitmap.SetPixel(x, y, matrix.Cells[i, j] == null ? (eat[i, j] ? Color.White : Color.Green) : Color.Red);
                }
            }

            bitmap.Save($@"C:\temp\bitmaps\{id}.bmp", ImageFormat.Bmp);
        }
    }
}