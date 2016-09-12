namespace ImpossibleCreatures.Settings
{
    internal enum ChildCreationPrice
    {
        LinearPenality,
        LogarithmicPenality,
        Constant
    }

    internal enum FoodBehavior
    {
        Grow,
        Plain
    }

    internal enum FoodDistibutionStrategy
    {
        AsWaterFromCorners,
        RandomRain,
        FillEntireField
    }
}
