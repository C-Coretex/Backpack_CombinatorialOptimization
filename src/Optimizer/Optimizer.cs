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

        private readonly object _lock = new();
        //Simulated Annealing
        //Don't see a reason to use Tabu Search here, since we remove random items, we add best-fit items with some randomness and we have many items to choose from.
        public Domain CalculateSolution(ICollection<double> temperatures, int degree = 4)
        {
            var bestDomain = BuildSolution();
            var currentDomain = bestDomain;
            var iteration = 0;

            Parallel.ForEach(temperatures, new ParallelOptions { MaxDegreeOfParallelism = degree },
                temperature =>
                {
                    Interlocked.Increment(ref iteration);

                    var domain = Advance(currentDomain, (int)Math.Ceiling(currentDomain.Items.Where(x => x.Value is null).Count() / 20.0), Math.Max(0.5, (double)iteration / temperatures.Count));
                    var domainTotalScore = domain.TotalScore();
                    if (domainTotalScore > bestDomain.TotalScore())
                    {
                        lock (_lock)
                        {
                            //we are checking again, because properties could be changed by another thread
                            //we don't want the lock to be outside of the if, since it would be a bottleneck and we want to do lock as rare as possible
                            if (domainTotalScore > bestDomain.TotalScore())
                                bestDomain = domain;
                        }
                    }

                    var currentDomainTotalScore = currentDomain.TotalScore();

                    var randomValue = Random.Shared.NextDouble();
                    var exp = Math.Exp((domainTotalScore - currentDomainTotalScore) / temperature);
                    if (domainTotalScore > currentDomainTotalScore || randomValue < exp)
                    {
                        lock (_lock)
                        {
                            //we are checking again, because properties could be changed by another thread
                            //we don't want the lock to be outside of the if, since it would be a bottleneck and we want to do lock as rare as possible
                            if(domainTotalScore > currentDomainTotalScore || randomValue < exp)
                                currentDomain = domain;
                        }
                    }

                    if (iteration % 500 == 0)
                        Console.WriteLine($"Iteration {iteration}, best score: {bestDomain.TotalScore()}");
                });

            return bestDomain;
        }


        //First Fit algorithm
        private Domain BuildSolution(Domain? domain = null, double minRand = 0.7)
        {
            domain = domain?.GetSnapshot() ?? _originalDomain.GetSnapshot();

            while (true)
            {
                var unassignedItems = domain.Items.Where(i => i.Value is null).Select(kv => kv.Key)
                    .OrderByDescending(i => i.Value * Random.Shared.NextDouble(minRand, 1))
                    .Select(i => (canAssign: domain.CanAssignItem(i, out var p), item: i, person: p))
                    .FirstOrDefault(x => x.canAssign);

                if (unassignedItems.person is null)
                    break;

                domain = domain.AssignItem(unassignedItems.item, unassignedItems.person);
            }

            return domain;
        }

        private Domain Advance(Domain domain, int itemCountToRemove = 10, double minRand = 0.5)
        {
            var items = domain.Items.Where(i => i.Value is not null).Select(kv => kv.Key).ToList();
            var itemsToRemove = new List<Item>(itemCountToRemove);
            for (var i = 0; i < itemCountToRemove && items.Count > 0; i++)
            {
                //random removal
                var removeIndex = Random.Shared.Next(items.Count);

                itemsToRemove.Add(items[removeIndex]);
                items.RemoveAt(removeIndex);
            }

            domain = domain.AssignItems(itemsToRemove.Select(i => (i, (Person?)null)));
            domain = BuildSolution(domain, minRand);

            return domain;
        }
    }
}
