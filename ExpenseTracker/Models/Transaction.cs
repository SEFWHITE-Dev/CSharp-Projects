using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        // CategoryId - Foreign Key
        // The default option is 0, but should not be allowed to be used
        [Range(1, int.MaxValue, ErrorMessage ="Please select a Category.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; } // navigation property to act as a foreign key 

        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public string? Note { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now; // set the default datetime value

        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get 
            { 
                return Category == null ? "" : Category.Icon + " " + Category.Title;
            }
        }

        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                // change the prefix to + or - based on the transaction type
                return ((Category == null || Category.Type == "Expense") ? "- " : "+ ") + Amount.ToString("C0");
            }
        }
    }
}
