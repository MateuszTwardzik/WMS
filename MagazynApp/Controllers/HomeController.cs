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
using MagazynApp.ViewModel;
using Microsoft.EntityFrameworkCore;

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
            var products = _context.Product
               .Include(p => p.Type)
               .AsNoTracking();
          //  var n = "Chemia";
          //  var test = products.Where(p => p.Type.Name.Equals(n)).Count();
            

            var typeName = _context.ProductType;

            var lstModel = new List<ProductsChartViewModel>();
            var typeList = new List<string>();
            var productList = new Dictionary<string, int>();

            foreach (var type in typeName)
            {
                typeList.Add(type.Name);
            }
            
            foreach(var type in typeList)
            {
                productList.Add(type, products.Where(p => p.Type.Name.Equals(type)).Count());
            }


            foreach (var product in productList)
            {
                lstModel.Add(new ProductsChartViewModel
                {
                    Name = product.Key,
                    Quantity = product.Value
                }); ;
            }

            //foreach (var type in typeName)
            //{
            //    lstModel.Add(new ProductsChartViewModel
            //    {
            //        Name = type.Name,
            //        // Quantity = products.Where(p => p.Type.Name.Equals(name)).Count()
            //        //Quantity = products.Count(p => p.Type.Name.Equals(type.Name))
            //        Quantity = 10
            //    });;
            //}


            //lstModel.Add(new ProductsChartViewModel
            //{
            //    Name = "Chemia",
            //    Quantity = test
            //});
            //lstModel.Add(new ProductsChartViewModel
            //{
            //    Name = "water",
            //    Quantity = 10
            //});
            return View(lstModel);
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
