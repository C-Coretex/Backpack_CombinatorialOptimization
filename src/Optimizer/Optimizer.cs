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
           var bestDomain = BuildInitial();

            //TODO: add not only max iterations, but also min delta...
            //TODO: add parallelization...
            iterationsMade = 0;
            for (var i = 0; i < maxIterations; i++)
            {
                iterationsMade = i + 1;
                //do advance
                //var canAdvance = domain.CanAdvance();
                //var domainTemp = domain.AssignItem()
                //var score = domainTemp.TotalScore();
            }

            return bestDomain;
        }

        //First Fit algorithm
        public Domain BuildInitial()
        {
            var domain = _originalDomain.GetSnapshot();

            while(true)
            {
                var unassignedItems = domain.Items.Where(i => i.Value is null).Select(kv => kv.Key).OrderByDescending(i => i.Value)
                    .Select(i => (canAssign: domain.CanAssignItem(i, out var p), item: i, person: p))
                    .Where(x => x.canAssign)
                    .OrderByDescending(x => domain.AssignItem(x.item, x.person).TotalScore()) //TODO: this might be slow...
                    .FirstOrDefault();
                
                if(unassignedItems.person is null)
                    break;

                domain = domain.AssignItem(unassignedItems.item, unassignedItems.person);
            }

            return domain;
        }
    }
}
