using MagazynApp.Data;
using MagazynApp.Models;
using MagazynApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly MagazynContext _context;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(MagazynContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }

        public ViewResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(shoppingCartViewModel);
        }

        public async Task<RedirectToActionResult> AddToShoppingCartAsync(int productId, int amount)
        {
            var selectedProduct = _context.Product.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct != null)
            {
                await _shoppingCart.AddToCartAsync(selectedProduct, amount);
            }
            return RedirectToAction("Index");
        }

        //public RedirectToActionResult AddToShoppingCart(int productId, int amount)
        //{
        //    var selectedProduct = _context.Product.FirstOrDefault(p => p.Id == productId);
        //    if (selectedProduct != null)
        //    {
        //       _shoppingCart.AddToCart(selectedProduct, amount);
        //    }
        //    return RedirectToAction("Index");
        //}

        public RedirectToActionResult RemoveFromShoppingCart(int productId)
        {
            var selectedProduct = _context.Product.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct != null)
            {
                _shoppingCart.RemoveFromCart(selectedProduct);
            }
            return RedirectToAction("Index");
        }
    }
}
