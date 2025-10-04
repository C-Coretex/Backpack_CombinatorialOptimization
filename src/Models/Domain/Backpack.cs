using Backpack_CombinatorialOptimization.Helpers;

namespace Backpack_CombinatorialOptimization.Models.Domain
{
    internal readonly record struct Backpack
    {
        public double MaxWeight { get; init; }
        public double MaxVolume { get; init; }
        public Backpack(double maxWeight, double maxVolume)
        {
            MaxWeight = maxWeight;
            MaxVolume = maxVolume;
        }

        public static Backpack CreateRandom(Random random)
        {
            var maxWeight = random.NextDouble(10, 70);
            return new(maxWeight, maxVolume: random.NextDouble(maxWeight / 2, 80));
        }
    }
}
