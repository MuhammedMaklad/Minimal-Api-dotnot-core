using System.Text.Json.Serialization;

namespace MinimalApi;

public class BaseResponse<T> where T : class
{
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public bool? Success { get; set; }


  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Error { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Message { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public T? Data { get; set; }
  
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public ICollection<T>? ArrayData { get; set; }
}
