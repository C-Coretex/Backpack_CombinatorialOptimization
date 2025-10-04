using Backpack_CombinatorialOptimization.Models.Domain;


Console.WriteLine("Hello, World!");

var backpackPool = new List<Backpack>
{
	new Backpack(1, maxWeight: 20, maxVolume: 30),
	new Backpack(2, maxWeight: 50, maxVolume: 70),
    new Backpack(3, maxWeight: 80, maxVolume: 90),
};
Console.WriteLine("Pool of backpacks:");
Console.WriteLine(string.Join(Environment.NewLine, backpackPool));

Console.WriteLine("--------------------------------------");
var domain = new Domain(backpackPool);
domain.Init(peopleCount: 1, itemCount: 100);
Console.WriteLine("Domain:");
Console.WriteLine("People (backpack id):");
Console.WriteLine(string.Join(", ", domain.People.Select(x => x.Backpack.Id).Order()));
Console.WriteLine();
Console.WriteLine("Items (top 20 by value):");
Console.WriteLine(string.Join(Environment.NewLine, domain.Items.OrderByDescending(x => x.Value).Take(20)
    .Select(x => $"Weight: {Math.Round(x.Weight, 2)}; Volume: {Math.Round(x.Volume, 2)}; Value: {x.Value}")));



Console.ReadKey();
/*
Domain:
	All available items (weight, size, value)
	Limitation:
		A backpack has max weight and max size. We have several people with the backpacks (there are several backpack types, e.g. for children, adults).
		We want to take as much value as we can, but we should account max optimal weight. If max optimal weight is the same, ideally we would prefer to have similar weight on two backpacks.
		

Scoring:
	Sum of all scores in all backpacks. Calculate difference between weights of the same type of backpacks and multiply by some parameter.

Move:
	Remove one item from a backpack, try to add new ones

Algorithm:
	...

 */