using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Repositories
{
    public class OrderStateRepository : IOrderStateRepository
    {
        private readonly MagazynContext _context;
        private readonly IOrderRepository _orderRepository;
        public OrderStateRepository(MagazynContext context, IOrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }
        public async Task ChangeStateAsync(Order order, int stateId)
        {
            order.StateId = stateId;
            order.State = _context.OrderState.FirstOrDefault(o => o.Id == stateId);
            order.CompletionDate = DateTime.Now;
            _context.Order.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
