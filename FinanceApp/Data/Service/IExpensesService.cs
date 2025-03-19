using FinanceApp.Models;

namespace FinanceApp.Data.Service
{
    public interface IExpensesService
    {
        Task<IEnumerable<Expense>> GetAll();
        Task Add(Expense expense);
        // IQueryable used to query collections, ideal for LINQ queries, as all the querying will happen in the db
        IQueryable GetChartData();
    }
}
