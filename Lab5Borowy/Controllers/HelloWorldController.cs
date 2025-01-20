using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace Lab5Borowy.Controllers
{
    public class HelloWorldController : Controller
    {
        //
        // GET: /HelloWorld/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hellur " + name;
            ViewData["NumTimes"] = numTimes;
            return View();
        }

        //
        //GET /HelloWorld/Welcome/
        //public string Welcome()
        //{
        //    return "This is the Welcome action method...";
        //}

        //
        //GET /HelloWorld/Welcome/
        // Requires using System.Text.Encodings.Web;
        //https://localhost:{PORT}/HelloWorld/Welcome?name=Rick&numtimes=4
        //public string Welcome(string name, int numTimes = 1)
        //{
        //    return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        //}

        // https://localhost:{PORT}/HelloWorld/Welcome/3?name=Rick
        //public string Welcome(string name, int ID = 1)
        //{
        //    return HtmlEncoder.Default.Encode($"Hello {name}, ID: {ID}");
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
