using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {

        // Every public method in a controller is callable as an HTTP endpoint
        // controller methods are 'action methods', and generally return something derived from ActionResult

        // GET: /HelloWorld/
        // returns a view result obj
        // uses a view template to generate HTML response
        public IActionResult Index()
        {
            //return "default action";
            return View();
        }

        // GET: /HelloWorld/Welcome/
        // optional param defaults to 1 if no value is passed in
        // uses HtmlEncoder.Default.Encode to protect the app from malicious input e.g. JS
        //public string Welcome(string name, int numTimes = 1)
        //{
        //    return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is : {numTimes}");
        //}

        // the ViewData dictionary is a dynamic obj
        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;
            return View();
        }

    }
}
