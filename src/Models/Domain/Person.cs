namespace Backpack_CombinatorialOptimization.Models.Domain
{
    internal record Person
    {
        public Backpack Backpack { get; init; }
        public List<Item> Items { get; init; } = [];

        public Person(Backpack backpack)
        {
            Backpack = backpack;
        }
    }
}
