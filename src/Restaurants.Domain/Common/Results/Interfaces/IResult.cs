namespace Restaurants.Domain.Common.Results.Interfaces;
public interface IResult
{
	List<Error>? Errors { get; }
}

public interface IResult<out TValue> : IResult
{
	TValue Value { get; }
}