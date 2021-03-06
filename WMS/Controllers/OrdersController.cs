using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MagazynApp.Data;
using MagazynApp.Models;
using MagazynApp.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using MagazynApp.Exceptions;

namespace MagazynApp.Controllers
{
    [Authorize (Roles = "Admin, User")]
    public class OrdersController : Controller
    {
        private readonly MagazynContext _context;

        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly IProductRepository _productRepository;
        private readonly IOrderStateRepository _orderStateRepository;

        public OrdersController
        (IOrderRepository orderRepository,
            ShoppingCart shoppingCart,
            IProductRepository productRepository,
            IOrderStateRepository orderStateRepository,
            MagazynContext context)
        {
            _orderRepository = orderRepository;
            _orderStateRepository = orderStateRepository;
            _productRepository = productRepository;
            _shoppingCart = shoppingCart;
            _context = context;
        }
        
        public async Task<IActionResult> Index(string filter)
        {
            return filter switch
            {
                "Uncompleted" => View(await _orderRepository.OrdersUncompleted()),
                "Completed" => View(await _orderRepository.OrdersCompleted()),
                _ => View(await _orderRepository.OrdersToListAsync())
            };
        }
        

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.FindOrderByIdAsync(id);

            ViewData["OrderItems"] = order.OrderLines.ToList();

            return View(order);
        }

        public IActionResult Checkout()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Fullname");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", order.ClientId);
            var items = await _shoppingCart.GetShoppingCartItemsAsync();
            _shoppingCart.ShoppingCartItems = items;


            if (!_shoppingCart.ShoppingCartItems.Any())
            {
                ModelState.AddModelError("", "Your card is empty, add some products first");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderRepository.CreateOrderAsync(order);
                    _shoppingCart.ClearCart();
                    return RedirectToAction("CheckoutComplete");
                }
                catch (MissingOrderItemsException)
                {
                    return RedirectToAction("CheckoutFail");
                }
            }

            return View(order);
        }
        

        public IActionResult CheckoutFail()
        {
            return View();
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

            var order = await _orderRepository.FindOrderByIdAsync(orderId);

            if (order.StateId == 1)
            {
                await _orderRepository.CompleteOrder(order);
                await _orderStateRepository.ChangeStateAsync(order, 2);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderRepository.DeleteOrder(id);
            return RedirectToAction("Index");
        }
        
    }
}