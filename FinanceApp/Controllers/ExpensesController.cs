using FinanceApp.Data;
using FinanceApp.Data.Service;
using FinanceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers
{
    public class ExpensesController : Controller
    {
        // define IExpensesService obj to allow communication to service, which communicates with the db
        private readonly IExpensesService _expensesService;

        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }

        public async Task<IActionResult> Index()
        {
            // wait for a response from the service, which retreives data from the db
            var expenses = await _expensesService.GetAll();
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
                // wait for the response from the Service, which retreives data from db 
                await _expensesService.Add(expense);

                // once the changes go through, direct user to Index page
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        public IActionResult GetChart()
        {
            // query the data using the service
            var data = _expensesService.GetChartData();
            return Json(data);
        }
    }
}
