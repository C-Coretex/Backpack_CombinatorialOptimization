using Backpack_CombinatorialOptimization.Models.Domain;

namespace Backpack_CombinatorialOptimization.Optimizer
{
    internal class Optimizer
    {
        private readonly Domain _originalDomain;
        public Optimizer(Domain domain)
        {
            _originalDomain = domain;
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

        private Domain BuildInitial()
        {
            return _originalDomain;
        }
    }
}
