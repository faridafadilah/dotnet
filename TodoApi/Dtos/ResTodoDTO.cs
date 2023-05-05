using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Dtos;

public class ResTodoDTO
{
  public long Id { get; set; }
  public string? Name { get; set; }
  public bool IsComplete { get; set; } 
  [NotMapped] 
  public IEnumerable<BookDTO> Books {get; set;}
}