using System.Text.Json.Serialization;

namespace MinimalApi;

public class Response<T> : BaseResponse
{
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public T? Data { get; set; }

  public static Response<T> Success(T Data) => new Response<T>
  {
    Data = Data
  };


  public static Response<T> Fail(string errorCode, string error) => new Response<T>
  {
    ErrorCode = errorCode,
    Error = error
  };
}

