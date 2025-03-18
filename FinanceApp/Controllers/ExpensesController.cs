using FinanceApp.Data;
using FinanceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers
{
    public class ExpensesController : Controller
    {
        // define context obj to allow communication to db
        private readonly FinanceAppContext _context;

        public ExpensesController(FinanceAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // wait for a response from the db
            var expenses = await _context.Expenses.ToListAsync();
            return View(expenses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            // check if valid expense 
            if (ModelState.IsValid)
            {
                // add changes to db
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();

                // once the changes go through, direct user to Index page
                return RedirectToAction("Index");
            }
            return View(expense);
        }
    }
}
