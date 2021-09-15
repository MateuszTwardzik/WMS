using MagazynApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<Order> FindOrderByIdAsync(int? id);
        Task<IList<Order>> OrdersToListAsync();
        Task DeleteOrder(int orderId);
        Task CompleteOrder(Order order); 
    }
}
