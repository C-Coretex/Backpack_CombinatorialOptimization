using Backpack_CombinatorialOptimization.Models.Domain;
using Backpack_CombinatorialOptimization.Optimizer;
using System.Diagnostics;

#region Configs
var backpackPool = new List<Backpack>
{
    new Backpack(1, maxWeight: 20, maxVolume: 50),
    new Backpack(2, maxWeight: 50, maxVolume: 90),
    new Backpack(3, maxWeight: 80, maxVolume: 120),
};

var countOfPeople = 10;
var itemCount = 1000;

var maxIterations = 10_000;

#endregion

Console.WriteLine("Hello, World!");


Console.WriteLine("Pool of backpacks:");
Console.WriteLine(string.Join(Environment.NewLine, backpackPool));

Console.WriteLine("--------------------------------------");

var domain = new Domain(backpackPool);
domain.Init(countOfPeople, itemCount);

Console.WriteLine("Domain:");
Console.WriteLine("People (backpack id):");
Console.WriteLine(string.Join(", ", domain.People.Select(x => x.Key.Backpack.Id).Order()));
Console.WriteLine();
Console.WriteLine("Items (top 20 by value):");
Console.WriteLine(string.Join(Environment.NewLine, domain.Items.OrderByDescending(x => x.Value).Take(20)
    .Select(x => $"Weight: {Math.Round(x.Key.Weight, 2)}; Volume: {Math.Round(x.Key.Volume, 2)}; Value: {x.Value}")));

var optimizer = new Optimizer(domain);
var sw = new Stopwatch();

Console.WriteLine("Calculating solution...");
sw.Start();
var solution = optimizer.CalculateSolution(maxIterations: maxIterations, out var iterations);
sw.Stop();
Console.WriteLine($"Solution calculated in {sw.ElapsedMilliseconds} ms, iterations made: {iterations}, total score: {solution.TotalScore()}");

Console.WriteLine("Solution:");
foreach (var person in solution.People.OrderBy(x => x.Key.Backpack.Id))
{
	var items = solution.People[person.Key];

	var totalWeight = items.Sum(i => i.Weight);
	var totalVolume = items.Sum(i => i.Volume);
	var totalValue = items.Sum(i => i.Value);

	Console.WriteLine($"Person with backpack id {person.Key.Backpack.Id}: items count: {items.Count}, total weight: {Math.Round(totalWeight, 2)}/{person.Key.Backpack.MaxWeight}, total volume: {Math.Round(totalVolume, 2)}/{person.Key.Backpack.MaxVolume}, total value: {totalValue}");
	foreach (var item in items.OrderByDescending(i => i.Value))
	{
		Console.WriteLine($"\tWeight: {Math.Round(item.Weight, 2)}; Volume: {Math.Round(item.Volume, 2)}; Value: {item.Value}");
	}
}

Console.ReadKey();
/*
Domain:
	All available items (weight, size, value)
	Limitation:
		A backpack has max weight and max size. We have several people with the backpacks (there are several backpack types, e.g. for children, adults).
		We want to take as much value as we can. 
		Hard Constraint: we cannot exceed max weight and max size of a backpack.
		Soft Constraint: If max optimal weight is the same, ideally we would prefer to have similar weight on two backpacks.
		

Scoring:
	Sum of all scores in all backpacks. Calculate difference between weights of the same type of backpacks and multiply by some parameter.

Move:
	Remove one item from a backpack, try to add new ones

Algorithm:
	...

 */