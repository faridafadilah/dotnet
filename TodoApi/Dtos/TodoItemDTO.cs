namespace TodoApi.Dtos;

public class TodoItemDTO
{
  public long Id { get; set; }
  public string? Name { get; set; }
  public bool IsComplete { get; set; }  
  public IFormFile? File { get; set; }
}