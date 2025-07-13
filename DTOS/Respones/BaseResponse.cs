using System.Text.Json.Serialization;

namespace MinimalApi;

public class BaseResponse
{
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public bool? Success { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? ErrorCode { get; set; }


  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Error { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Message { get; set; }
}
