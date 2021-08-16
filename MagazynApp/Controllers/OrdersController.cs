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
using Newtonsoft.Json;

namespace MagazynApp.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Index()
        {
            return View(await _orderRepository.OrdersToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.FindOrderByIdAsync(id);


            List<OrderDetail> OrderItems = new List<OrderDetail>();

            foreach (var orderDetail in order.OrderLines)
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
        public async Task<IActionResult> Checkout(Order order)
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", order.ClientId);
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            List<MissingOrderedProduct> missingItemsList = new List<MissingOrderedProduct>();
            foreach(var item in items)
            {
                if(item.Amount > _productRepository.FindProductByIdAsync(item.Product.Id).Result.Quantity)
                {
                    var product = await _productRepository.FindProductByIdAsync(item.Product.Id);
                    var missingItem = new MissingOrderedProduct()
                    {
                        ProductId = item.Product.Id,
                        ProductAmount = product.Quantity,
                        OrderId = order.Id,
                        OrderedAmount = item.Amount
                    };
                    missingItemsList.Add(missingItem);
                    _context.Add(missingItem);
                }
            }

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your card is empty, add some products first");
            }

            if (ModelState.IsValid)
            {
                if (missingItemsList != null)
                {
                    TempData["missingItemsList"] = JsonConvert.SerializeObject(missingItemsList);
                    TempData["ClientId"] = order.ClientId;
                    return RedirectToAction("CheckoutConfirmation");
                }
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            return View(order);
        }
        [HttpGet]
        public IActionResult CheckoutConfirmation()
        {
            if(TempData["missingItemsList"] is string s)
            {
                var missingItems = JsonConvert.DeserializeObject<List<MissingOrderedProduct>>(s);
                List<MissingOrderedProductViewModel> missingItemsList = new List<MissingOrderedProductViewModel>();
                foreach(var item in missingItems)
                {
                    var missingItem = new MissingOrderedProductViewModel()
                    {
                        ProductName = _productRepository.FindProductByIdAsync(item.ProductId).Result.Name,
                        ProductAmount = item.ProductAmount,
                        OrderedAmount = item.OrderedAmount,
                        MissingAmount = item.MissingAmount
                    };
                    missingItemsList.Add(missingItem);
                }
                TempData["missingItemsList"] = JsonConvert.SerializeObject(missingItems);
                return View(missingItemsList);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CheckoutConfirmation(Order order)
        {
            order.ClientId = int.Parse(TempData["ClientId"].ToString());

            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            if (ModelState.IsValid)
            {
                _orderRepository.CreateOrder(order);
                if (TempData["missingItemsList"] is string s)
                {
                    var missingItems = JsonConvert.DeserializeObject<List<MissingOrderedProduct>>(s);
                    foreach (var item in missingItems)
                    {
                        item.OrderId = order.Id;
                        _context.MissingOrderedProduct.Add(item);
                    }
                   await _context.SaveChangesAsync();
                }

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
            Dictionary<int, int> compStock = new Dictionary<int, int>();

            var order = await _orderRepository.FindOrderByIdAsync(orderId);

            if (order.StateId == 1)
            {
                foreach (var detail in order.OrderLines)
                {
                    var product = await _productRepository.FindProductByIdAsync(detail.ProductId);
                    compStock.Add(product.Quantity, detail.Quantity);
                }
                if (!CheckStock(compStock))
                {
                    ModelState.AddModelError("", "Brak wystarczającej ilości produktów");
                    TempData["StockError"]= "Brak wystarczającej ilości produktów!";
                }
                else
                {
                    foreach (var detail in order.OrderLines)
                    {
                        var product = await _productRepository.FindProductByIdAsync(detail.ProductId);
                        var productStock = product.Quantity - detail.Quantity;
                        await _productRepository.SetAmountAsync(product.Id, productStock);
                    }
                    await _orderStateRepository.ChangeState(order.Id, 2);

                }

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderRepository.DeleteOrder(id);
            return RedirectToAction("Index");
        }
        private bool CheckStock(Dictionary<int, int> compStock)
        {
            foreach (var detail in compStock)
            {
                if (detail.Key < detail.Value)
                {
                    return false;
                }
            }
            return true;
        }

        [HttpGet]
        public IActionResult MissingProducts()
        {
            var missingItems =  _context.MissingOrderedProduct;
            List<MissingOrderedProductViewModel> missingItemsList = new List<MissingOrderedProductViewModel>();
            foreach (var item in missingItems)
            {
                var missingItem = new MissingOrderedProductViewModel()
                {
                    ProductName = _productRepository.FindProductByIdAsync(item.ProductId).Result.Name,
                    ProductAmount = item.ProductAmount,
                    OrderedAmount = item.OrderedAmount,
                    MissingAmount = item.MissingAmount
                };
                missingItemsList.Add(missingItem);
            }
            return View(missingItemsList);
        }
    }
}
