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

        public Domain CalculateSolution(int iterations)
        {
            var bestDomain = BuildSolution();

            for (var i = 0; i < iterations; i++)
            {
                var domain = Advance(bestDomain);
                if (domain.TotalScore() > bestDomain.TotalScore())
                    bestDomain = domain;

                if ((i + 1) % 100 == 0)
                    Console.WriteLine($"Iteration {i + 1}, best score: {bestDomain.TotalScore()}");
            }

            return bestDomain;
        }

        //Simulated Annealing - change trashold + change multiplication randomness in BuildSolution
        //I've chosen Simululated Annealing, because I also want to implement parallelization and it's easier to implement, since this algorithm doesn't require memory
        //Don't see reason to use Tabu Search here, since we remove random items, we add best-fit items with some randomness and we have many items to choose from.
        public Domain CalculateSolution(IEnumerable<double> temperatures)
        {
            var bestDomain = BuildSolution();
            var currentDomain = bestDomain;
            //TODO: add not only max iterations, but also min delta...
            //TODO: add parallelization...
            var iteration = 0;

            //CalculateSolution starting temperature
            /*
            var costDiff = 0.0;
            for (var i = 0; i < 100; i++)
            {
                bestDomain = Advance(currentDomain);
                costDiff += Math.Abs(currentDomain.TotalScore() - bestDomain.TotalScore());
                currentDomain = bestDomain;
            }
            costDiff /= 100;
            var t = -costDiff / Math.Log(0.8); //80% chance to accept worse solution at start
            */
            foreach (var temperature in temperatures)
            {
                iteration++;

                var domain = Advance(currentDomain);
                var domainTotalScore = domain.TotalScore();
                if (domainTotalScore > bestDomain.TotalScore())
                    bestDomain = domain;


                var currentDomainTotalScore = currentDomain.TotalScore();

                /*Console.WriteLine("-------");
                Console.WriteLine(Random.Shared.NextDouble());
                Console.WriteLine(Math.Pow(Math.E, (currentDomainTotalScore - domainTotalScore) / temperature));
                */
                if (domainTotalScore > currentDomainTotalScore 
                    || Random.Shared.NextDouble() < Math.Exp((domainTotalScore - currentDomainTotalScore) / temperature))
                    currentDomain = domain;

                if (iteration % 100 == 0)
                    Console.WriteLine($"Iteration {iteration}, best score: {bestDomain.TotalScore()}");
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
                    .OrderByDescending(x => domain.AssignItem(x.item, x.person).TotalScore() * Random.Shared.NextDouble(0.8, 1)) //TODO: this might be slow...
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
