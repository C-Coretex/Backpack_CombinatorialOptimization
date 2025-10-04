namespace Backpack_CombinatorialOptimization.Models.Domain
{
    internal record Domain
    {
        private const int totalValueMultiplier = 100;
        private const int unevenWeightPenaltyMultiplier = 10;

        private readonly IReadOnlyCollection<Backpack> _backpackPool = [];
        public Dictionary<Item, Person?> Items { get; private set; } = [];
        public Dictionary<Person, HashSet<Item>> People { get; private set; } = [];
        public Domain(ICollection<Backpack> backpacks)
        {
            _backpackPool = (IReadOnlyCollection<Backpack>)backpacks;
        }

        public void Init(int peopleCount, int itemCount)
        {
            People = Enumerable.Range(0, peopleCount)
                .ToDictionary(_ => new Person(_backpackPool.ElementAt(Random.Shared.Next(0, _backpackPool.Count))), _ => new HashSet<Item>());
            Items = Enumerable.Range(0, itemCount)
                .ToDictionary(_ => Item.CreateRandom(Random.Shared), _ => (Person?)null);
        }

        public Domain AssignItem(Item item, Person? person)
        {
            var previousPerson = Items[item];
            if (previousPerson == person)
                return this;

            var snapshot = GetSnapshot();

            if (previousPerson is not null)
                snapshot.People[previousPerson].Remove(item);

            snapshot.Items[item] = person;
            if (person is not null)
                snapshot.People[person].Add(item);

            return snapshot;
        }

        public bool CanAssignItem(Item item, Person person)
        {
            var items = People[person];
            if(items.Contains(item))
                return false;

            var totalWeight = items.Sum(i => i.Weight) + item.Weight;
            if(totalWeight > person.Backpack.MaxWeight)
                return false;

            var totalVolume = items.Sum(i => i.Volume) + item.Volume;
            if(totalVolume > person.Backpack.MaxVolume)
                return false;

            return true;
        }

        public Domain AssignItems(params IEnumerable<(Item item, Person? person)> items)
        {
            var snapshot = new Lazy<Domain>(() => GetSnapshot());
            foreach (var (item, person) in items)
            {
                var previousPerson = Items[item];
                if (previousPerson == person)
                    continue;

                if (previousPerson is not null)
                    snapshot.Value.People[previousPerson].Remove(item);

                snapshot.Value.Items[item] = person;
                if(person is not null)
                    snapshot.Value.People[person].Add(item);
            }
            return snapshot.IsValueCreated ? snapshot.Value : this;
        }

        public double TotalScore()
        {
            var totalValue = People.Values.SelectMany(x => x).Sum(i => i.Value) * totalValueMultiplier;
            var totalWeightPenalty = People.GroupBy(p => p.Key.Backpack.Id)
                .Select(g => g.Sum(kv => (kv.Key.Backpack.MaxWeight - kv.Value.Sum(x => x.Weight))))
                .Sum(x => x * unevenWeightPenaltyMultiplier);

            return totalValue - totalWeightPenalty;
        }

        private Domain GetSnapshot()
        {
            return this with
            {
                Items = new Dictionary<Item, Person?>(Items),
                People = new Dictionary<Person, HashSet<Item>>(People)
            };
        }
    }
}
