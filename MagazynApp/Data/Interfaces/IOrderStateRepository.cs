using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Interfaces
{
    public interface IOrderStateRepository
    {
        Task ChangeState(int orderId, int stateId);
    }
}
