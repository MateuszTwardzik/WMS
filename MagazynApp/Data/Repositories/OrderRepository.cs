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
        private readonly IWarehouseRepository _warehouseRepository;

        public OrderRepository
        (MagazynContext context,
            ShoppingCart shoppingCart,
            IProductRepository productRepository,
            IWarehouseRepository warehouseRepository)
        {
            _context = context;
            _shoppingCart = shoppingCart;
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
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

        public async Task CompleteOrder(Order order)
        {
            var socketProductList = await FindSocket(order.OrderLines.ToList());
            var updateSockets = new List<Socket>();
            var deleteProductSockets = new List<SocketProduct>();
            var changedProductSockets = new List<SocketProduct>();
            var updateProductSockets = new List<SocketProduct>();

            foreach (var detail in order.OrderLines)
            {
                double temp = detail.Quantity;
                
                var detailSockets = socketProductList.Where(s => s.ProductId == detail.ProductId)
                    .OrderBy(s => s.Amount)
                    .ToList();


                foreach (var socket in detailSockets)
                {
                    if (temp > socket.Amount)
                    {
                        socket.Socket.Capacity -= socket.Amount;
                        updateSockets.Add(socket.Socket);
                        temp -= socket.Amount;

                        socket.Amount -= socket.Amount;
                        changedProductSockets.Add(socket);
                    }
                    else
                    {
                        socket.Socket.Capacity -= temp;
                        updateSockets.Add(socket.Socket);

                        socket.Amount -= temp;
                        changedProductSockets.Add(socket);
                    }
                }
            }

            foreach (var changedSocket in changedProductSockets)
            {
                if (changedSocket.Amount == 0)
                {
                    deleteProductSockets.Add(changedSocket);
                }
                else
                {
                    updateProductSockets.Add(changedSocket);
                }
            }

            _context.UpdateRange(updateSockets);
            _context.UpdateRange(updateProductSockets);
            _context.RemoveRange(deleteProductSockets);
            await _context.SaveChangesAsync();
        }

        private async Task<List<SocketProduct>> FindSocket(List<OrderDetail> orderDetails)
        {
            var socketProductList = new List<SocketProduct>();
            var socketList = new List<Socket>();


            foreach (var detail in orderDetails)
            {
                double productSocket = 0;
                
                var sockets = _warehouseRepository.SocketProductToList().Result
                    .Where(s => s.ProductId == detail.ProductId)
                    .OrderBy(s => s.Amount)
                    .ToList();


                foreach (var socket in sockets)
                {
                    if (productSocket < detail.Quantity)
                    {
                        socketProductList.Add(socket);
                        productSocket += socket.Amount;
                    }
                }
            }

            return socketProductList;
        }
    }
}