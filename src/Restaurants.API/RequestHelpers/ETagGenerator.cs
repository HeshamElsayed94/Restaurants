using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Restaurants.API.RequestHelpers;

public static class ETagGenerator
{
	public static string GenerateETag(object ob)
	{
		string json = JsonSerializer.Serialize(ob);
		byte[] bytes = Encoding.UTF8.GetBytes(json);
		byte[] hash = SHA256.HashData(bytes);
		return $"\"{Convert.ToBase64String(hash)}\"";
	}
}