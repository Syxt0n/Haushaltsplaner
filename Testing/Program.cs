using Infrastructure.Repositories;

using Domain.Repositories;
using Domain.Food;

internal class Program
{
	private static void Main(string[] args)
	{
		IFoodRepository rep = new FoodRepositories();

		FoodAggregate test = Task.WhenAll(rep.GetByIdAsync(
			new Guid("d8248fa3-91d7-47c4-b0eb-d10d6841ce1d")
		)).Result.First();

		Console.WriteLine($"{test.ID}, {test.Name}, {test.Ingredients}");
	}
}