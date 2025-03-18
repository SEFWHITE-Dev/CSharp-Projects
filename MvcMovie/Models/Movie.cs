using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models
{
    public class Movie
    {
        public int Id { get; set; } // pk for db

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Title { get; set; } // string? means the property is nullable

        [Display(Name = "Release Date")] // specifies what to display for the name of the field
        [DataType(DataType.Date)] // specifies the ReleaseDate var to be of a certain type. only date is displayed (not time). no user input is required.
        public DateTime ReleaseDate { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")] // limit what characters can be used for input
        [Required] // user must input something (but can still input white space)
        [StringLength(30)]
        public string? Genre { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [StringLength(5)]
        [Required]
        public string? Rating { get; set; }

    }
}
