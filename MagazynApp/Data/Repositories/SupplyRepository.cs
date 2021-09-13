using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagazynApp.Exceptions;

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
            return await _context.Supply.Include(s => s.State).Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.Id == supplyId);
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
                await StoreSupply(supply);
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

        public async Task StoreSupply(Supply supply)
        {
            double supplyAmount = supply.Amount;

            var socketList = FindSockets(supply.Amount)
                .Result.OrderBy(s => s.Id);

            

            var socketProduct = new List<SocketProduct>();
            try
            {
                foreach (var socket in socketList)
                {
                    var socketAmount = socket.MaxCapacity - socket.Capacity;
                    double productAmount = 0;


                    if (socketProduct == null)
                    {
                        throw new SocketNotFoundException();
                    }

                    if (socketAmount < supplyAmount)
                    {
                        socket.Capacity += socketAmount;
                        productAmount = socketAmount;
                        supplyAmount -= socketAmount;
                    }
                    else
                    {
                        socket.Capacity += supplyAmount;
                        productAmount = supplyAmount;
                    }

                    socketProduct.Add(new SocketProduct()
                    {
                        ProductId = supply.ProductId,
                        SocketId = socket.Id,
                        Amount = productAmount
                    });
                }

                await _context.AddRangeAsync(socketProduct);
                _context.UpdateRange(socketList);
                await _context.SaveChangesAsync();
            }
            catch (SocketNotFoundException)
            {
            }
        }

        private async Task<List<Socket>> FindSockets(double amount)
        {
            var socketList = new List<Socket>();
            while (socketList.Sum(s => s.MaxCapacity - s.Capacity) < amount)
            {
                var freeSocket = await _context.Socket.FirstAsync(s => s.Capacity < 40 && !socketList.Contains(s));
                if (freeSocket == null)
                {
                    throw new SocketNotFoundException();
                }

                socketList.Add(freeSocket);
            }

            return socketList;
        }
    }
}