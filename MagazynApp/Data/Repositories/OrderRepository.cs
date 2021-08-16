using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Order> FindOrderByIdAsync(int? id)
        {
            var order = await _context.Order
                .Include(o => o.Client)
                .Include(o => o.State)
                .Include(o => o.OrderLines)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            return order;
        }
        public async Task<IList<Order>> OrdersToListAsync()
        {
            var orderList = await _context.Order.Include(o => o.Client).Include(o => o.State).ToListAsync();
            return orderList;
        }

        public async Task DeleteOrder(int orderId)
        {
            var order = await FindOrderByIdAsync(orderId);
            foreach(var detail in order.OrderLines)
            {
                _context.OrderDetail.Remove(detail);
            }
            
            
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

}
