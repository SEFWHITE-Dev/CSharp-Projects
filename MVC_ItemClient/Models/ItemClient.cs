namespace MVC_App.Models
{
    public class ItemClient
    {
        // To establish a many-to-many relationship, a helper class is used between the two 
        // The respective ID's are used as the foreign keys

        // FK for the item
        public int ItemId { get; set; }
        public Item Item { get; set; }

        // FK for the client
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
