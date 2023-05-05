using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos;

public class RegisterDto
{
  [Required]
  public string firstName { get; set; }
  [Required]
  public string lastName { get; set; }
  [Required]
  public string username { get; set; }
  [Required]
  public string email { get; set; }
  [Required]
  public string password { get; set; }
  public string role { get; set; }
}