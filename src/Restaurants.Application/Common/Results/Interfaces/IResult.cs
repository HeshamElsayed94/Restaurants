using Restaurants.Application.Common.Results.Errors;

namespace Restaurants.Application.Common.Results.Interfaces;
public interface IResult
{
	List<Error>? Errors { get; }

	bool ISuccess { get; }
}

public interface IResult<out TValue> : IResult
{
	TValue Value { get; }
}