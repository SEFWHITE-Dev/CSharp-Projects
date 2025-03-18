using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_App.Data;
using MVC_App.Models;

namespace MVC_App.Controllers
{
    public class ItemsController : Controller
    {
        //public IActionResult Overview()
        //{
        //    var item = new Item() { Name="keybaord"};

        //    return View(item);
        //}

        //// we can take the 'int id' param because in the program.cs file the MapControllerRoute() takes a 
        //// {id?} as a part of its URL param. hence the same name can be used and referenced
        //public IActionResult Edit(int id)
        //{
        //    return Content("id: " + id);
        //}


        private readonly MVC_AppContext _context;


        public ItemsController(MVC_AppContext context)
        {
            _context = context;
        }


        // asynchronous method so the execution waits for the db response before proceeding
        public async Task<IActionResult> Index()
        {
            // query items in the db
            var item = await _context.Items.Include(s => s.SerialNumber)
                .Include(c => c.Category)
                .Include(ic => ic.ItemClients) // connect the ItemClient helper model
                .ThenInclude(c => c.Client) // after the above has connected, connect to the client model
                .ToListAsync(); // get a list of all the items from db

            return View(item);
        }


        public IActionResult Create()
        {
            // display all the categories
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }


        // for post requests, the [HttpPost] is required. Get requests by default don't need one
        // 'Bind' the data to the Item
        [HttpPost] 
        public async Task<IActionResult> Create([Bind("Id, Name, Price, CategoryId")] Item item)
        {
            // check is input Model is actually an Item
            if (ModelState.IsValid)
            {
                _context.Items.Add(item); // add it to the db
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(item);
        }


        public async Task<IActionResult> Edit(int id)
        {
            // display all the categories
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            // look through the db for the specific item
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Price, CategoryId")] Item item) 
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(item);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }


        // when the user sends a Delete request of type Post, this method is called
        // ActionName is defined when multiple of the same action name is used
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            
            if(item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
