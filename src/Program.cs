using Backpack_CombinatorialOptimization.Models.Domain;


var backpackPool = new List<Backpack>
{
	new Backpack(maxWeight: 20, maxVolume: 30),
	new Backpack(maxWeight: 50, maxVolume: 70),
    new Backpack(maxWeight: 80, maxVolume: 90),
};

var domain = new Domain(backpackPool);
domain.Init(peopleCount: 1, itemCount: 100);






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