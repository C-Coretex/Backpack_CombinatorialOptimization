using Backpack_CombinatorialOptimization.Helpers;
using Backpack_CombinatorialOptimization.Models.Domain;

namespace Backpack_CombinatorialOptimization.Optimizer
{
    internal class Optimizer
    {
        private readonly Domain _originalDomain;
        public Optimizer(Domain domain)
        {
            _originalDomain = domain.GetSnapshot();
        }

        public Domain CalculateSolution(int maxIterations, out int iterationsMade)
        {
            var bestDomain = BuildSolution();

            //TODO: add not only max iterations, but also min delta...
            //TODO: add parallelization...
            iterationsMade = 0;
            for (var i = 0; i < maxIterations; i++)
            {
                iterationsMade = i + 1;

                var domain = Advance(bestDomain);
                if(domain.TotalScore() > bestDomain.TotalScore())
                    bestDomain = domain;

                if ((i + 1) % 100 == 0)
                    Console.WriteLine($"Iteration {i + 1}, best score: {bestDomain.TotalScore()}");
            }

            return bestDomain;
        }

        //First Fit algorithm
        private Domain BuildSolution(Domain? domain = null)
        {
            domain = domain?.GetSnapshot() ?? _originalDomain.GetSnapshot();

            while (true)
            {
                var unassignedItems = domain.Items.Where(i => i.Value is null).Select(kv => kv.Key).OrderByDescending(i => i.Value)
                    .Select(i => (canAssign: domain.CanAssignItem(i, out var p), item: i, person: p))
                    .Where(x => x.canAssign)
                    .OrderByDescending(x => domain.AssignItem(x.item, x.person).TotalScore() * Random.Shared.NextDouble(0.85, 1)) //TODO: this might be slow...
                    .FirstOrDefault();

                if (unassignedItems.person is null)
                    break;

                domain = domain.AssignItem(unassignedItems.item, unassignedItems.person);
            }

            return domain;
        }

        private Domain Advance(Domain domain, int itemCountToRemove = 10)
        {
            var items = domain.Items.Where(i => i.Value is not null).Select(kv => kv.Key).ToList();
            List<Item> itemsToRemove = [];
            for (var i = 0; i < itemCountToRemove && items.Count > 0; i++)
            {
                //random removal
                var removeIndex = Random.Shared.Next(items.Count);

                itemsToRemove.Add(items[removeIndex]);
                items.RemoveAt(removeIndex);
            }

            domain = domain.AssignItems(itemsToRemove.Select(i => (i, (Person?)null)));
            domain = BuildSolution(domain);

            return domain;
        }
    }
}
