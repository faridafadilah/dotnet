using System.ComponentModel.DataAnnotations.Schema;
using TodoApi.Base;
namespace TodoApi.Models;

public class TodoItem : BaseModel
{
  public string? Name { get; set; }
  public bool IsComplete { get; set; }
  [NotMapped]
  public IEnumerable<Book> Books {get; set;}

  // properti untuk menyimpan informasi file
    public string? FileName { get; set; } // menyimpan nama file
    public string? FilePath { get; set; } // menyimpan path file di server
    [NotMapped]
    public IFormFile? File { get; set; } // menyimpan file yang 
}