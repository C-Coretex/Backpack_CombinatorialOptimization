using Backpack_CombinatorialOptimization.Helpers;

namespace Backpack_CombinatorialOptimization.Models.Domain
{
    internal readonly record struct Item
    {
        public int Id { get; init; }
        public double Weight { get; init; }
        public double Volume { get; init; }
        public int Value { get; init; }

        public Item(int id, double weight, double volume, int value)
        {
            Id = id;
            Weight = weight;
            Volume = volume;
            Value = value;
        }

        public static Item CreateRandom(Random random)
        {
            var weight = random.NextDouble(0.5, 10);
            var volume = random.NextDouble(1, 15);

            var parameter = (int)(Math.Ceiling((weight + volume) / 2));
            return new(random.Next(), weight, volume, value: random.Next(parameter, 30 + parameter));
        }
    }
}
