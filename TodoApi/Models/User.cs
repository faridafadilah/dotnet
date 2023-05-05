using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models;
public class User
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long id {get; set;}
  public string firstName { get; set; }
  public string lastName { get; set; }
  public string username { get; set; }
  [EmailAddress]
  public string email { get; set; }
  public byte[] passwordHash { get; set; }
  public byte[] passwordSalt { get; set; }
  public string role { get; set; }
}