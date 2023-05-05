using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;
  public class AuthenticatieModel 
  {
    [Required]
    public string username {get; set;}
    [Required]
    public string password {get; set;}
  }
