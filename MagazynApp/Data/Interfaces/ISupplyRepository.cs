using MagazynApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Interfaces
{
    public interface ISupplyRepository
    {
        Task CreateSupplyAsync(int productId, int amount );
        Task<Supply> FindSupplyAsync(int supplyId);
        Task<List<Supply>> SupplyToListAsync();
        Task CompleteSupplyAsync(Supply supply);
        Task DeleteSupplyAsync(int supplyId);
    }
}
