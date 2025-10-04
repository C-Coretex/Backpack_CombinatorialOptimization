using Backpack_CombinatorialOptimization.Helpers;

namespace Backpack_CombinatorialOptimization.Models.Domain
{
    internal readonly record struct Backpack
    {
        public int Id { get; init; }
        public double MaxWeight { get; init; }
        public double MaxVolume { get; init; }
        public Backpack(int id, double maxWeight, double maxVolume)
        {
            Id = id;
            MaxWeight = maxWeight;
            MaxVolume = maxVolume;
        }

        public static Backpack CreateRandom(Random random)
        {
            var maxWeight = random.NextDouble(10, 70);
            return new(random.Next(), maxWeight, maxVolume: random.NextDouble(maxWeight / 2, 80));
        }
    }
}
