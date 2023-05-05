using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models;

public class Book {
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
  public string? Name {get; set;}
  public string? Hal {get; set;}
  [ForeignKey("Todo")]
  public long TodoId {get; set;}
  public TodoItem Todo {get; set;}
}