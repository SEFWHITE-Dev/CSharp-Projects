﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        // CategoryId - Foreign Key
        public int CategoryId { get; set; }
        public Category Category { get; set; } // navigation property to act as a foreign key 

        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public string? Note { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now; // set the default datetime value

    }
}
