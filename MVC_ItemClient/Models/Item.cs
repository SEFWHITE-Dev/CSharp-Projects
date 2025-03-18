using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_App.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; // init with temp null value (null forgiving -- we don't want it to be null but it will be taken care of at a later stage)
        public double Price { get; set; }
        public int? SerialNumberId { get; set; } // var? since it doesn't need to be set as soon as its init
        public SerialNumber? SerialNumber { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        // connect the Item model to the ItemClient helper model
        public List<ItemClient>? ItemClients { get; set; }
    }
    
}
