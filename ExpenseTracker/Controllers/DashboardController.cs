using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Transactions;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : Controller
    {
        // Set up db connectivity in the constructor
        private readonly ApplicationDBContext _context;

        // constructor to init the db context
        public DashboardController(ApplicationDBContext context) {
            _context = context;
        }

        // main action for the Dashboard page
        public async Task<IActionResult> Index()
        {
            // Show the last 7 days transactions
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Models.Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category) // perform SQL JOIN with the Category table
                .Where(y => y.DateTime >= StartDate && y.DateTime <= EndDate) // filter transitions within the date range
                .ToListAsync();

            /*
                Equivalent SQL Query:

                SELECT t.*, c.*
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryId = c.CategoryId
                WHERE t.DateTime BETWEEN @StartDate AND @EndDate;
            */

            
            // Total income
            int TotalIncome = SelectedTransactions.Where(i => i.Category.Type == "Income").Sum(s =>  s.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

            // Total Expenses
            int TotalExpenses = SelectedTransactions.Where(i => i.Category.Type == "Expense").Sum(s => s.Amount);
            ViewBag.TotalExpense = TotalExpenses.ToString("C0");

            // Balance
            int Balance = TotalIncome - TotalExpenses;

            // Ensure negative values are formatted correctly
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-Uk");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);

            // Area chart - expense by category
            ViewBag.AreaChartData = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),

                })
                .OrderByDescending(l => l.amount); // order categories by highest expense


            /*
                Equivalent SQL Query:

                SELECT c.CategoryId, CONCAT(c.Icon, ' ', c.Title) AS categoryTitleWithIcon,
                       SUM(t.Amount) AS amount
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryId = c.CategoryId
                WHERE c.Type = 'Expense'
                GROUP BY c.CategoryId
                ORDER BY amount DESC;
            */


            // Spline chart - income vs expense
            // Income summary
            List<SpineChartData> IncomeSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.DateTime)
                .Select(k => new SpineChartData
                {
                    day = k.First().DateTime.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();


            /*
                Equivalent SQL Query:

                SELECT CONVERT(VARCHAR, t.DateTime, 106) AS day, SUM(t.Amount) AS income
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryId = c.CategoryId
                WHERE c.Type = 'Income'
                GROUP BY CONVERT(VARCHAR, t.DateTime, 106);
            */



            // Expense summary
            List<SpineChartData> ExpenseSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.DateTime)
                .Select(k => new SpineChartData
                {
                    day = k.First().DateTime.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            // Combine income and expense lists by date
            // there may be days where the are no transactions
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            // Creates a query that will loop through each day in the Last7Days array
            ViewBag.SpineChartData = from day in Last7Days
                                     // left JOIN --joins each day from Last7Days with any matching day in IncomeSummary list
                                     // if a match is found, add it to the corresponding income value of that day
                                     join income in IncomeSummary on day equals income.day 
                                     into dayIncomeJoined 
                                     from income in dayIncomeJoined.DefaultIfEmpty() // handle days with no income
                                     join expense in ExpenseSummary on day equals expense.day // left JOIN
                                     into expenseJoined
                                     from expense in expenseJoined.DefaultIfEmpty() // handle days with no expenses
                                     select new
                                     {
                                         day = day,
                                         income = income == null ? 0 : income.income,
                                         expense = expense == null ? 0 : expense.expense,
                                     };

            //Recent Transactions
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(j => j.DateTime)
                .Take(5)
                .ToListAsync();


            return View();
        }
    }

    public class SpineChartData
    {
        public string day;
        public int income;
        public int expense;
    }
}
