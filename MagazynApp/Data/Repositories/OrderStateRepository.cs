using MagazynApp.Data.Interfaces;
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
        public async Task ChangeState(int orderId, int stateId)
        {
            var order = await _orderRepository.FindOrderByIdAsync(orderId);
            order.StateId = 2;
            _context.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
