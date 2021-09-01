using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Repositories
{
    public class SupplyRepository : ISupplyRepository
    {
        private readonly MagazynContext _context;
        public SupplyRepository(MagazynContext context)
        {
            _context = context;
        }
        public async Task CreateSupplyAsync(int productId, int amount)
        {
            var supply = new Supply
            {
                ProductId = productId,
                Amount = amount,
                StateId = 1,
                OrderDate = DateTime.Now

            };
            await _context.AddAsync(supply);
            await _context.SaveChangesAsync();
        }
        public async Task<Supply> FindSupplyAsync(int supplyId)
        {
            return await _context.Supply.Include(s => s.State).Include(s => s.Product).FirstOrDefaultAsync(s => s.Id == supplyId);
        }
        public async Task<List<Supply>> SupplyToListAsync()
        {
            return await _context.Supply.Include(s => s.State).Include(s => s.Product).ToListAsync();
        }

        public async Task CompleteSupplyAsync(Supply supply)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                supply.StateId = 2;
                supply.State = await _context.SupplyState.FirstOrDefaultAsync(s => s.Id == supply.StateId);
                supply.CompletionDate = DateTime.Now;
                supply.Product.Quantity += supply.Amount;
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();

            }
        }
        public async Task DeleteSupplyAsync(int supplyId)
        {
            _context.Supply.Remove(await FindSupplyAsync(supplyId));
            await _context.SaveChangesAsync();
        }
    }
}
