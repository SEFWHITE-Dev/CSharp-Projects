using FinanceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data
{
    public class FinanceAppContext : DbContext
    {
        public FinanceAppContext(DbContextOptions<FinanceAppContext> options): base(options) 
        {

        }

        // for each model class we create in the project, need to set an instance
        // the instance that will be used to interact with the "Expenses" table in the db 
        public DbSet<Expense> Expenses { get; set; }
    }
}
