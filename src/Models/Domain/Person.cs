namespace Backpack_CombinatorialOptimization.Models.Domain
{
    internal record Person
    {
        public int Id { get; init; }
        public Backpack Backpack { get; init; }

        public Person(int id, Backpack backpack)
        {
            Id = id;
            Backpack = backpack;
        }
    }
}
