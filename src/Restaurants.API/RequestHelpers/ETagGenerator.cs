using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Restaurants.API.RequestHelpers;

public static class ETagGenerator
{
	public static string GenerateETag(object ob, bool weak = false)
	{
		string json = JsonSerializer.Serialize(ob);

		byte[] bytes = Encoding.UTF8.GetBytes(json);
		byte[] hash = weak ? MD5.HashData(bytes) : SHA256.HashData(bytes);
		string base64 = Convert.ToBase64String(hash);

		return weak ? $"W/\"{base64}\"" : $"\"{base64}\"";
	}
}