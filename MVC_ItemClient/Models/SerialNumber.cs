using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_App.Models
{
    public class SerialNumber
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ItemId { get; set; } // var? allows it to be null

        [ForeignKey("ItemId")]
        public Item? item { get; set; }
    }
}
