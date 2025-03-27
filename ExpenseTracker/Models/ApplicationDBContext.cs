using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Models
{
    public class ApplicationDBContext : DbContext
    {
        // constructor must have the db provider passed into it, and the corresponding connection string
        // pass inthe same "options" param to the base() class for DbContext, as this class inherits it
        // the instance of this obj will be created through DI
        public ApplicationDBContext(DbContextOptions options) : base(options)  
        {

        }

        // the dbcontext can only create tables that have been defined here
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
