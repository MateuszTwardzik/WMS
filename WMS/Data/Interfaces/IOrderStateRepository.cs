using MagazynApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Interfaces
{
    public interface IOrderStateRepository
    {
        Task ChangeStateAsync(Order order, int stateId);
    }
}
