namespace Backpack_CombinatorialOptimization.Helpers
{
    public static class RandomHelpers
    {
        public static double NextDouble(this Random random, double minValue, double maxValue)
            => random.NextDouble() * (maxValue - minValue) + minValue;
    }
}
