using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagazynApp.Data;
using MagazynApp.Models;
using MagazynApp.Data.Interfaces;
using MagazynApp.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace MagazynApp.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly MagazynContext _context;

        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;

        public OrdersController(IOrderRepository orderRepository, ShoppingCart shoppingCart, MagazynContext context)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var magazynContext = _context.Order.Include(o => o.Client).Include(o => o.State);
            return View(await magazynContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Client)
                .Include(o => o.State)
                .Include(o => o.OrderLines)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);


            List<OrderDetail> OrderItems = new List<OrderDetail>();

            foreach(var orderDetail in order.OrderLines)
            {
                OrderItems.Add(orderDetail);
            }
            ViewData["OrderItems"] = OrderItems;

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult Checkout()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Fullname");
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", order.ClientId);
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your card is empty, add some products first");
            }

            if (ModelState.IsValid)
            {
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Zamówienie zostało złozone!";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int? orderId)
        {

            if (orderId == null)
            {
                return NotFound();
            }
            var products = await _context.Product.ToListAsync();

            var order = await _context.Order
                .Include(o => o.Client)
                .Include(o => o.State)
                .Include(o => o.OrderLines)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == orderId);
            if (order.StateId == 1)
            {
                foreach (var detail in order.OrderLines)
                {
                    var productStock = products.FirstOrDefault(p => p.Id == detail.ProductId);
                    productStock.Quantity = productStock.Quantity - detail.Quantity;
                    await SetAmount(detail.ProductId, productStock.Quantity);
                }
                order.StateId = 2;
            }
            return View(order);
        }

        public async Task<Product> SetAmount(int productId, int amount)
        {
            var product = await _context.Product.FindAsync(productId);
            product.Quantity = amount;
            return product;
        }
    }
}
