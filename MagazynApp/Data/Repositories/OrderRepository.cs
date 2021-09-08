using MagazynApp.Data.Interfaces;
using MagazynApp.Exceptions;
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
        private readonly IProductRepository _productRepository;


        public OrderRepository(MagazynContext context, ShoppingCart shoppingCart, IProductRepository productRepository)
        {
            _context = context;
            _shoppingCart = shoppingCart;
            _productRepository = productRepository;
        }


        public async Task CreateOrderAsync(Order order)
        {
            order.OrderDate = DateTime.Now;
            order.StateId = 1;

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;
            var missingItems = shoppingCartItems.Where(x => x.Amount > x.Product.Quantity);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (missingItems.Any())
                {
                    throw new MissingOrderItemsException(missingItems.Select(x => x.ProductId).ToList());
                }

                order.OrderLines = shoppingCartItems.Select(x => new OrderDetail()
                {
                    Quantity = x.Amount,
                    ProductId = x.ProductId,
                    Price = x.Product.Price
                }).ToList();


                order.Quantity = order.OrderLines.Sum(x => x.Quantity);

                order.Price = order.OrderLines.Sum(x => x.Quantity * x.Price);

                await _context.Order.AddAsync(order);

                foreach (var detail in order.OrderLines)
                {
                    var product = await _productRepository.FindProductByIdAsync(detail.ProductId);
                    await _productRepository.SetAmountAsync(product.Id, product.Quantity - detail.Quantity);
                }

                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (MissingOrderItemsException)
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task<Order> FindOrderByIdAsync(int? id)
        {
            var order = await _context.Order
                .Include(o => o.Client)
                .Include(o => o.State)
                .Include(o => o.MissingOrderedProducts)
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
            if (order.StateId == 1)
            {
                foreach (var detail in order.OrderLines)
                {
                    var product = await _productRepository.FindProductByIdAsync(detail.ProductId);
                    var productStock = product.Quantity + detail.Quantity;
                    await _productRepository.SetAmountAsync(product.Id, productStock);
                }

            }
            _context.OrderDetail.RemoveRange(order.OrderLines);

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

}
