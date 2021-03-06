﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using CellsAutomate.Constants;
using CellsAutomate.Food.DistributingStrategy;
using CellsAutomate.Food.FoodBehavior;
using Creaturestests.Food;

namespace CellsAutomate.Food.Tests
{
    [TestClass()]
    public class FoodMatrixTests
    {

        [TestMethod()]
        public void HasOneBiteIsTrueTest()
        {
            var eatMatrix = new FoodMatrix(2, 2, 1, new FillingFromCornersByWavesStrategy(), new PlainBehaviour());
            var point = new Point(0, 0);

            FrequentlyUsedMethods.RaiseFoodLevelToConstantWithAddFood(eatMatrix, point, CreatureConstants.OneBite);

            Assert.IsTrue(eatMatrix.HasOneBite(point, CreatureConstants.OneBite));
        }

        [TestMethod()]
        public void HasOneBiteIsFalseTest()
        {
            var eatMatrix = new FoodMatrix(2, 2, 1, new FillingFromCornersByWavesStrategy(), new PlainBehaviour());

            for (int i = 0; i < eatMatrix.Length; i++)
            {
                for (int j = 0; j < eatMatrix.Height; j++)
                {
                    Assert.IsFalse(eatMatrix.HasOneBite(new Point(i, j), CreatureConstants.OneBite));
                }
            }
        }

        [TestMethod()]
        public void HasMaxFoodLevelIsTrueTest()
        {
            var eatMatrix = new FoodMatrix(2, 2, 1, new FillingFromCornersByWavesStrategy(), new PlainBehaviour());
            var point = new Point(0, 0);

            FrequentlyUsedMethods.RaiseFoodLevelToConstantWithAddFood(eatMatrix, point, FoodMatrixConstants.MaxFoodLevel);

            Assert.IsTrue(eatMatrix.HasMaxFoodLevel(point));
        }

        [TestMethod()]
        public void HasMaxFoodLevelIsFalseTest()
        {
            var eatMatrix = new FoodMatrix(2, 2, 1, new FillingFromCornersByWavesStrategy(), new PlainBehaviour());

            for (int i = 0; i < eatMatrix.Length; i++)
            {
                for (int j = 0; j < eatMatrix.Height; j++)
                {
                    if (FoodMatrixConstants.AddedFoodLevel < FoodMatrixConstants.MaxFoodLevel)
                        eatMatrix.AddFood(new Point(i, j));
                    Assert.IsFalse(eatMatrix.HasMaxFoodLevel(new Point(i, j)));
                }
            }
        }

        [TestMethod()]
        public void AddFoodTest()
        {
            var eatMatrix = new FoodMatrix(2, 2, 1, new FillingFromCornersByWavesStrategy(), new PlainBehaviour());
            var point = new Point(0, 0);

            FrequentlyUsedMethods.RaiseFoodLevelToConstantWithAddFood(eatMatrix, point, CreatureConstants.OneBite);

            Assert.IsTrue(eatMatrix.HasOneBite(point, CreatureConstants.OneBite));
        }

        [TestMethod()]
        public void TakeFoodIsTrueTest()
        {
            var eatMatrix = new FoodMatrix(2, 2, 1, new FillingFromCornersByWavesStrategy(), new PlainBehaviour());
            var point = new Point(0, 0);

            FrequentlyUsedMethods.RaiseFoodLevelToConstantWithAddFood(eatMatrix, point, CreatureConstants.OneBite);

            Assert.IsTrue(eatMatrix.HasOneBite(point, CreatureConstants.OneBite));

            var counter = FoodMatrixConstants.AddedFoodLevel / CreatureConstants.OneBite;
            for (int i = 0; i < counter; i++)
            {
                Assert.IsTrue(eatMatrix.TakeFood(point, CreatureConstants.OneBite));
            }

            Assert.IsFalse(eatMatrix.HasOneBite(point, CreatureConstants.OneBite));
        }

        [TestMethod()]
        public void TakeFoodIsFalseTest()
        {
            var eatMatrix = new FoodMatrix(2, 2, 1, new FillingFromCornersByWavesStrategy(), new PlainBehaviour());
            var point = new Point(0, 0);
            Assert.IsFalse(eatMatrix.TakeFood(point, CreatureConstants.OneBite));
        }
    }
}