using System.Text.Json.Serialization;

namespace MinimalApi;

public class BaseResponse<T> where T : class
{
  public bool? Success { get; set; }


  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Error { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public ICollection<string>? Errors { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Message { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public T? Data { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public ICollection<T>? ArrayData { get; set; }


  public static BaseResponse<T> OK(string message, T? data = null) => new()
  {
    Success = true,
    Message = message,
    Data = data,
  };

  public static BaseResponse<T> OK(string message, ICollection<T>? arrayData) => new()
  {
    Success = true,
    Message = message,
    ArrayData = arrayData
  };
  public static BaseResponse<T> Failure(string message, string? error = null) => new()
  {
    Success = false,
    Message = message,
    Error = error,
  };
  public static BaseResponse<T> Failure(string message, ICollection<string> errors) => new()
  {
    Success = false,
    Message = message,
    Errors = errors
  };
}
