using Backpack_CombinatorialOptimization.Helpers;

namespace Backpack_CombinatorialOptimization.Models.Domain
{
    internal readonly record struct Item
    {
        public double Weight { get; init; }
        public double Volume { get; init; }
        public int Value { get; init; }

        public Item(double weight, double volume, int value)
        {
            Weight = weight;
            Volume = volume;
            Value = value;
        }

        public static Item CreateRandom(Random random)
            => new(weight: random.NextDouble(0.3, 10), volume: random.NextDouble(0.3, 10), value: random.Next(1, 25));
    }
}
