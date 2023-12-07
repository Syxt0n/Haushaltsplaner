using Infrastructure.Repositories;

using Domain.Repositories;
using Domain.Food;

internal class Program
{
	private static void Main(string[] args)
	{
		IFoodRepository rep = new FoodRepositories("Host=federlein.website;Username=admin;Password=Lindach1210;Port=5432;Database=Haushaltsplaner;Trust Server Certificate=True");

		FoodAggregate test = Task.WhenAll(rep.GetByIdAsync(
			new Guid("c36f8a0a-4832-4f0b-a590-724b06d8e260")
		)).Result.First();

		Console.WriteLine($"{test.ID}, {test.Name}, {test.Ingredients}");
	}
}