namespace Restaurants.Application.Restaurants.Queries.Caching;
public static class RestaurantCachingTags
{
	public const string Main = "Restaurants";

	public const string Paged = "PagedRestaurants";

	public static IEnumerable<string> Single(int id) => [Paged, $"{Main}:{id}"];
}