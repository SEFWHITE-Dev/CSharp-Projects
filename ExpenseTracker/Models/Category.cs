﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Column(TypeName ="nvarchar(50)")]
        [Required(ErrorMessage = "Enter a Title")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public string Icon { get; set; } = ""; // default as empty string

        [Column(TypeName = "nvarchar(10)")]
        public string Type { get; set; } = "Expense"; // default as Expense

        // Not relevant to the db tables
        [NotMapped]
        public string? TitleWithIcon
        {
            get 
            {
                return this.Icon + " " + this.Title;
            }
        }
    }
}
