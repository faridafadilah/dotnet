using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models
{
  public class Movie
  {
    public int Id { get; set; }
    [StringLength(60, MinimumLength =3), Required]
    public string? Title { get; set; }

    [DataType(DataType.Date), Display(Name = "Release Date")]
    public DateTime ReleaseDate { get; set; }

    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$"), StringLength(30), Required]
    public string? Genre { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
  }
}