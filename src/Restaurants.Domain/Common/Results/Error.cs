using System.Net;

namespace Restaurants.Domain.Common.Results;
public readonly record struct Error
{
	public string Code { get; }
	public string Description { get; }
	public HttpStatusCode StatusCode { get; }

	private Error(string code, string description, HttpStatusCode statusCode)
	{
		Code = code;
		Description = description;
		StatusCode = statusCode;
	}

	public static Error Failure(string code, string description = "General failure.")
		=> new(code, description, HttpStatusCode.InternalServerError);

	public static Error Unexpected(string code = nameof(Unexpected), string description = "Unexpected error.")
  => new(code, description, HttpStatusCode.InternalServerError);

	public static Error Validation(string code = nameof(Validation), string description = "Validation error")
		=> new(code, description, HttpStatusCode.UnprocessableEntity);

	public static Error Conflict(string code = nameof(Conflict), string description = "Conflict error")
		=> new(code, description, HttpStatusCode.Conflict);

	public static Error NotFound(string code = nameof(NotFound), string description = "Not found error")
		=> new(code, description, HttpStatusCode.NotFound);

	public static Error Unauthorized(string code = nameof(Unauthorized), string description = "Unauthorized error")
		=> new(code, description, HttpStatusCode.Unauthorized);

	public static Error Forbidden(string code = nameof(Forbidden), string description = "Forbidden error")
		=> new(code, description, HttpStatusCode.Forbidden);

	public static Error Create(int type, string code, string description)
		=> new(code, description, (HttpStatusCode)type);
}