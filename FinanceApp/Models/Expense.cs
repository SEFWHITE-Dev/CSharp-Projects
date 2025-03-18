using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models
{
    public class Expense
    {
        public int Id { get; set; } // generated automatically

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage ="Input must be greater than 0")] // set restrictions on what can be inputted
        public double Amount { get; set; }

        [Required]
        public string Category { get; set; } = null!;

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
