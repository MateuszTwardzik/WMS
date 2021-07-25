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

namespace MagazynApp.Controllers
{
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

        public IActionResult Checkout()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id");
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
            ViewBag.CheckoutCompleteMessage = "Thanks for your order! :) ";
            return View();
        }
    }
}
