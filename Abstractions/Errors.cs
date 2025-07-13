namespace MinimalApi;

public class Errors
{
  public static readonly Error IdMissMatch =  new ("DifferentId", "The provided ID does not match the expected ID.");
  public static readonly Error NotFound = new ("NotFound", "The requested resource was not found.");
}
