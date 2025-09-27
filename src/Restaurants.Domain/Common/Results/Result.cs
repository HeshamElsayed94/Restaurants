using System.ComponentModel;
using System.Text.Json.Serialization;
using Restaurants.Domain.Common.Results.Interfaces;

namespace Restaurants.Domain.Common.Results;
public class Result<TValue> : IResult<TValue>
{
	private readonly TValue? _value;

	public TValue Value => _value!;

	public List<Error>? Errors { get; }

	[JsonConstructor]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("For serialize only", true)]
	public Result(TValue? value, List<Error>? errors)
	{
		if (value is not null)
		{
			_value = value;
		}
		else
		{
			if (errors == null || errors.Count == 0)
				throw new ArgumentException("Provide at least one error.", nameof(errors));
			Errors = errors;
		}
	}

	private Result(TValue value)
	{
		if (value is null)
			throw new ArgumentNullException(nameof(value));

		_value = value;
	}

	private Result(Error error) => Errors = [error];

	private Result(List<Error> errors)
	{

		if (errors is null || errors.Count == 0)
			throw new ArgumentException("Cannot create Errors from an empty collection of errors. Provide at least one error.", nameof(errors));

		Errors = errors;
	}

	public static implicit operator Result<TValue>(TValue value) => new(value);

	public static implicit operator Result<TValue>(Error error) => new(error);

	public static implicit operator Result<TValue>(List<Error> errors) => new(errors);

	public TNextValue Match<TNextValue>(Func<TValue, TNextValue> onValue, Func<List<Error>, TNextValue> onError)
		=> _value is null ? onError(Errors!) : onValue(Value);
}

public readonly record struct Void;