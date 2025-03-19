using FinanceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data.Service
{
    public class ExpensesService : IExpensesService
    {
        private readonly FinanceAppContext _context;

        public ExpensesService(FinanceAppContext context)
        {
            _context = context;
        }

        public async Task Add(Expense expense)
        {
            // add changes to db
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<Expense>> GetAll()
        {
            // wait for a response from the db
            var expenses = await _context.Expenses.ToListAsync();
            return expenses;
        }

        public IQueryable GetChartData()
        {
            var data = _context.Expenses // filter the expenses
                .GroupBy(e => e.Category) // group by category
                .Select(g => new // create an anonymous obj
                {
                    Category = g.Key, // store the category
                    Total = g.Sum(e => e.Amount) // store the total amount for each category
                });

            return data;
        }
    }
}
