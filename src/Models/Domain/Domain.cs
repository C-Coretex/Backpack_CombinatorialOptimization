namespace Backpack_CombinatorialOptimization.Models.Domain
{
    internal record Domain
    {
        private IReadOnlyCollection<Backpack> _backpackPool = [];
        public IReadOnlyCollection<Person> People { get; private set; }
        public IReadOnlyCollection<Item> Items { get; private set; }

        public Domain(ICollection<Backpack> backpacks)
        {
            _backpackPool = (IReadOnlyCollection<Backpack>)backpacks;
        }

        public void Init(int peopleCount, int itemCount)
        {
            People = Enumerable.Range(0, peopleCount)
                .Select(_ => new Person(_backpackPool.ElementAt(Random.Shared.Next(0, _backpackPool.Count))))
                .ToArray();
            Items = Enumerable.Range(0, itemCount)
                .Select(_ => Item.CreateRandom(Random.Shared))
                .ToArray();
        }
    }
}
