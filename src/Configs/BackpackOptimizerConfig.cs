using System.Text.Json.Serialization;

namespace Backpack_CombinatorialOptimization.Configs
{
    [JsonSerializable(typeof(BackpackOptimizerConfig))]
    [JsonSerializable(typeof(BackpackConfig))]
    internal partial class SourceGenerationContext : JsonSerializerContext
    {
    }

    internal record BackpackOptimizerConfig
    {
        public IReadOnlyCollection<BackpackConfig> BackpackPool { get; init; } = [];
        public int PeopleCount { get; init; }
        public int ItemCount { get; init; }
        public int ParallelTasks { get; init; }
    }

    internal record BackpackConfig(int Id, int MaxWeight, int MaxVolume);
}
