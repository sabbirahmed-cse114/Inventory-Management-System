using Microsoft.AspNetCore.Mvc;
using REC.Inventory.Web.Models;
using System.Diagnostics;

namespace REC.Inventory.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMembership _membership;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger,
            IMembership membership, IEmailSender emailSender)
        {
            _logger = logger;
            _membership = membership;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
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
