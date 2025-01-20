using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Lab5Borowy.Models
{
    public class Book
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Title { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [StringLength(30)]
        public string? Author { get; set; }
        public string? Description { get; set; }
        
        public string[] Publishers { get; set; }

        [Display(Name = "Publish Date")]
        [DataType(DataType.Date)]
        public  DateTime PublishDate { get; set; }

        [Display(Name = "Number Of Pages")]
        public int NumberOfPages { get; set; }
        //try sth like [Column(TypeName = "decimal(18,2)")]
        public double Weight { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        //public string[] Tags { get; set; }
        public string? Rating { get; set; }
    }
}
