using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MagazynContext _context;
        private readonly ShoppingCart _shoppingCart;


        public OrderRepository(MagazynContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }


        public void CreateOrder(Order order)
        {
            order.OrderDate = DateTime.Now;
            order.StateId = 1;
            decimal total = 0;
            int quantity = 0;

            //order = _context.Order.Add(order).Entity;
            //_context.SaveChanges();            

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Quantity = shoppingCartItem.Amount,
                    ProductId = shoppingCartItem.Product.Id,
                    //OrderId = order.Id,
                    Price = shoppingCartItem.Product.Price
                };

                total += shoppingCartItem.Product.Price * shoppingCartItem.Amount;
                quantity += shoppingCartItem.Amount;


                //_context.OrderDetail.Add(orderDetail);
                order.OrderLines.Add(orderDetail);
            }
            order.Price = total;
            order.Quantity = quantity;
            _context.Order.Add(order);
            _context.SaveChanges();
        }
    }
}
