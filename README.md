# Combinatorial Optimization for Backpack problem

### Problem:
We are going on a trip in a group. We have children and adults in our group, and they have different backpack types. We want to maximize the value that we put into backpacks, but we also want people with the same backpack type to carry approximately similar weight, since if the weight is different, some people will get more tired than others.

### Domain:
	All available items (weight, size, value)
	Limitations:
		A backpack has max weight and max size. We have several people with the backpacks (there are several backpack types, e.g., for children, adults...).
		We want to take as much value as we can. 
		Hard Constraint: we cannot exceed a backpack's max weight and size.
		Soft Constraint: if several people have the same backpack type, we would prefer to have approximately similar weights that they carry.
		
### Scoring:
	Sum of all item values in all backpacks, subtracted by the difference between the weights of the same type of backpacks.

### Move:
	Remove several random items from random backpacks, add new items to the backpacks ones.

### Algorithm:
	First solution: First Fit algorithm
  Optimizer Algorithm: Simulated Annealing combined with some decreasing with time randomness when choosing the best solution
