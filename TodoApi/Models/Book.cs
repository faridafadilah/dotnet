using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoApi.Base;
namespace TodoApi.Models;

public class Book : BaseModel {
  public string? Name {get; set;}
  public string? Hal {get; set;}
  [ForeignKey("Todo")]
  public long TodoId {get; set;}
  public TodoItem Todo {get; set;}
}