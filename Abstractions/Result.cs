namespace MinimalApi;

public class Result
{
  public bool IsSuccess { get; set; }
  public bool IsFailed => !IsSuccess;

  public Error Error { get; set; }

  protected Result(bool isSuccess, Error error)
  {
    IsSuccess = isSuccess;
    Error = error;
  }

  public static Result Success() => new Result(true, Error.None);
  public static Result Failure(Error error) => new Result(false, error);
}
