using FinanceApp.Models;

namespace FinanceApp.Data.Service
{
    public class ExpensesService : IExpensesService
    {
        private readonly FinanceAppContext _context;

        public Task Add(Expense expense)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Expense>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
