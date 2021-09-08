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
using Microsoft.AspNetCore.Authorization;
using MagazynApp.Data.Interfaces;

namespace MagazynApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IOrderRepository _orderRepository;

        public HomeController(ILogger<HomeController> logger, MagazynContext context,
            IProductRepository productRepository,
            IProductTypeRepository productTypeRepository,
            IOrderRepository orderRepository)

        {
            _logger = logger;
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _orderRepository = orderRepository;
        }

        //public IActionResult Index()
        //{

        //    return View();
        //}
        public async Task<IActionResult> Index()
        {

            var products = _productRepository.GetProducts().ToList();
            var orders = await _orderRepository.OrdersToListAsync();

            ViewBag.products_number = products.Count().ToString();


            var productTypePieChart = products
                .GroupBy(p => p.Type.Name)
                .Select(p => new ProductsChartViewModel()
                {
                    Name = p.Key,
                    Quantity = p.Count()
                })
                .OrderBy(p => p.Name);

            var countedOrdersByDate = orders
                .GroupBy(o => o.OrderDate)
                .Select(o => new OrderLineChartViewModel()
                {
                    OrderDate = o.Key.ToShortDateString(),
                    Quantity = o.Count()
                })
                .OrderBy(o => o.OrderDate);

            DateTime StartDate = orders.Min(o => o.OrderDate);
            DateTime EndDate = DateTime.Now;
            
            var dates = Enumerable.Range(0, (EndDate - StartDate).Days + 1)
                .Select(day => StartDate.AddDays(day));


            var DatesWithOutOrders = dates.Select(d => new OrderLineChartViewModel()
            {
                OrderDate = d.ToShortDateString(),
                Quantity = 0
            });


            var orderLineChart = from noOrder in DatesWithOutOrders
                    join order in countedOrdersByDate
                    on noOrder.OrderDate equals order.OrderDate into dateGroup
                    from item in dateGroup.DefaultIfEmpty(new OrderLineChartViewModel { OrderDate = noOrder.OrderDate, Quantity = 0 })
                    select new OrderLineChartViewModel { OrderDate = noOrder.OrderDate, Quantity = item.Quantity };

            HomeChartsViewModel charts = new HomeChartsViewModel()
            {
                ProductTypesPieChart = productTypePieChart,
                OrderLineChart = orderLineChart
            };

            return View(charts);
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
