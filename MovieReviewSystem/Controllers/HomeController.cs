using Microsoft.AspNetCore.Mvc;
using MovieReviewSystem.Models;
using System.Diagnostics;

namespace MovieReviewSystem.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            Console.WriteLine("here");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
