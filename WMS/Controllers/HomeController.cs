using MagazynApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MagazynApp.Data;
using MagazynApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using MagazynApp.Data.Interfaces;

namespace MagazynApp.Controllers
{
    [Authorize (Roles = "Admin, User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ISupplyRepository _supplyRepository;
        private readonly IWarehouseRepository _warehouseRepository;

        public HomeController(ILogger<HomeController> logger, MagazynContext context,
            IProductRepository productRepository,
            IProductTypeRepository productTypeRepository,
            IOrderRepository orderRepository,
            IClientRepository clientRepository,
            ISupplyRepository supplyRepository,
            IWarehouseRepository warehouseRepository)

        {
            _logger = logger;
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _supplyRepository = supplyRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<IActionResult> Index()
        {
            //var products = _productRepository.GetProducts().ToList();

            var products = await _productRepository.ProductsToListAsync();
            var orders = await _orderRepository.OrdersToListAsync();
            var occupiedSpace = await _warehouseRepository.WarehouseOccupiedSpace();
            
            ViewBag.occupiedSpaceInt = Convert.ToInt32(occupiedSpace);
            ViewBag.occupiedSpaceDouble = occupiedSpace.ToString("F2");
            ViewBag.ordersCount = orders.Count;
            ViewBag.clientCount =  await _clientRepository.ClientsCount();
            ViewBag.productsCount = products.Count;
            ViewBag.suppliesCount = await _supplyRepository.SupplyCount();

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

            var startDate = DateTime.Now;

            if (orders.Count != 0)
            {
                startDate = orders.Min(o => o.OrderDate);
            }

            var endDate = DateTime.Now;

            var dates = Enumerable.Range(0, (endDate - startDate).Days + 1)
                .Select(day => startDate.AddDays(day));

            var datesWithOutOrders = dates.Select(d => new OrderLineChartViewModel()
            {
                OrderDate = d.ToShortDateString(),
                Quantity = 0
            });


            var orderLineChart = from noOrder in datesWithOutOrders
                join order in countedOrdersByDate
                    on noOrder.OrderDate equals order.OrderDate into dateGroup
                from item in dateGroup.DefaultIfEmpty(new OrderLineChartViewModel
                    {OrderDate = noOrder.OrderDate, Quantity = 0})
                select new OrderLineChartViewModel {OrderDate = noOrder.OrderDate, Quantity = item.Quantity};

            var charts = new HomeChartsViewModel()
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
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}