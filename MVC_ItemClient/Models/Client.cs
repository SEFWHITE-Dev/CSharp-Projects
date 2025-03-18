namespace MVC_App.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // connect the Item model to the ItemClient helper model
        public List<ItemClient>? ItemClients { get; set; }
    }
}
