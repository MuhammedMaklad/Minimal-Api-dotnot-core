using System.Text.Json.Serialization;

namespace MinimalApi;

public class AssignAdminResponse
{

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Message { get; set; }
  
  public int Code { get; set; }
}
