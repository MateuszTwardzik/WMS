using MagazynApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class ShoppingCart
    {
        private readonly MagazynContext _context;
        private ShoppingCart(MagazynContext context)
        {
            _context = context;
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<MagazynContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public async Task AddToCartAsync(Product product, int amount)
        {
            var shoppingCartItem =
                    await _context.ShoppingCartItems.SingleOrDefaultAsync(
                        s => s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId.ToString(),
                    ProductId = product.Id,
                    //Product = product,
                    Amount = amount
                };

                await _context.ShoppingCartItems.AddAsync(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount += amount;                
            }
            await _context.SaveChangesAsync();
        }

        //public void AddToCart(Product product, int amount)
        //{
        //    var shoppingCartItem =
        //             _context.ShoppingCartItems.SingleOrDefault(
        //                s => s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);

        //    if (shoppingCartItem == null)
        //    {
        //        shoppingCartItem = new ShoppingCartItem
        //        {
        //            ShoppingCartId = ShoppingCartId.ToString(),
        //            ProductId = product.Id,                 
        //            Amount = amount
        //        };

        //        _context.ShoppingCartItems.Add(shoppingCartItem);
        //    }
        //    else
        //    {
        //        shoppingCartItem.Amount += amount;
        //    }
        //    _context.SaveChanges();
        //}

        public void RemoveFromCart(Product product)
        {
            var shoppingCartItem =
                    _context.ShoppingCartItems.SingleOrDefault(
                        s => s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);


            if (shoppingCartItem != null)
            {
                _context.ShoppingCartItems.Remove(shoppingCartItem);
            }

            _context.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems =
                       _context.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.Product)
                           .ToList());
        }
        public async Task<List<ShoppingCartItem>> GetShoppingCartItemsAsync()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems = await
                        _context.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.Product)
                           .ToListAsync());
        }

        public void ClearCart()
        {
            var cartItems = _context
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId.ToString());

            _context.ShoppingCartItems.RemoveRange(cartItems);

            _context.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _context.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Product.Price * c.Amount).Sum();
            return total;
        }
    }
}
