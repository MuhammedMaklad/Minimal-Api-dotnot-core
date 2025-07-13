namespace MinimalApi;


public sealed record Error(string code, string? description = null)
{
  public static readonly Error None = new Error(string.Empty);
  public static implicit operator Result(Error error) => Result.Failure(error);
};

