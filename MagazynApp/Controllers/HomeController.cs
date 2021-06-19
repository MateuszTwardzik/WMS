using MagazynApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MagazynApp.Data;
using MagazynApp.Models;

namespace MagazynApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly MagazynContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MagazynContext context)
        {
            _logger = logger;
            _context = context;
        }

        //public IActionResult Index()
        //{
            
        //    return View();
        //}
        public async Task<IActionResult> Index()
        {
            ViewBag.products_number = _context.Product.Count().ToString();
            ViewBag.users_number = _context.User.Count();
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
