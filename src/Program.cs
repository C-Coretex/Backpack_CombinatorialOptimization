using Backpack_CombinatorialOptimization.Configs;
using Backpack_CombinatorialOptimization.Models.Domain;
using Backpack_CombinatorialOptimization.Optimizer;
using System.Diagnostics;
using System.Text.Json;

#region Configs
using var stream = File.OpenRead("config.json");
var config = await JsonSerializer.DeserializeAsync(
    stream,
    SourceGenerationContext.Default.BackpackOptimizerConfig
);

config ??= new BackpackOptimizerConfig
{
	BackpackPool = new[]
	{
		new BackpackConfig(1, 20, 30),
		new BackpackConfig(2, 40, 50),
		new BackpackConfig(3, 60, 70),
	},
	PeopleCount = 8,
	ItemCount = 300,
	ParallelTasks = 4
};

var backpackPool = config.BackpackPool
    .Select(b => new Backpack(b.Id, b.MaxWeight, b.MaxVolume))
    .ToList();

var countOfPeople = config.PeopleCount;
var itemCount = config.ItemCount;

List<double> temperatures = [];
var value = 3000.0;
while (value >= 0.0001)
{
	temperatures.AddRange(Enumerable.Repeat(value, 20));
    value *= 0.99;
}

//the higher the value, the faster the algorithm, but possibly the less optimal the solution
var parallelismDegree = config.ParallelTasks;

#endregion

Console.WriteLine("Pool of backpacks:");
Console.WriteLine(string.Join(Environment.NewLine, backpackPool));

Console.WriteLine("--------------------------------------");

var domain = new Domain(backpackPool);
domain.Init(countOfPeople, itemCount);

Console.WriteLine("Domain:");
Console.WriteLine("People (backpack id):");
Console.WriteLine(string.Join(", ", domain.People.Select(x => x.Key.Backpack.Id).Order()));
Console.WriteLine();
Console.WriteLine("Items (top 30 by value):");
Console.WriteLine(string.Join(Environment.NewLine, domain.Items.OrderByDescending(x => x.Key.Value).Take(30)
    .Select(x => $"Weight: {Math.Round(x.Key.Weight, 2)}; Volume: {Math.Round(x.Key.Volume, 2)}; Value: {x.Key.Value}")));

var optimizer = new Optimizer(domain);
var sw = new Stopwatch();

Console.WriteLine("Calculating solution...");
sw.Start();

var solution = optimizer.CalculateSolution(temperatures, parallelismDegree);

sw.Stop();
Console.WriteLine($"Solution calculated in {sw.ElapsedMilliseconds} ms, iterations made: {temperatures.Count}, total score: {solution.TotalScore()}");

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