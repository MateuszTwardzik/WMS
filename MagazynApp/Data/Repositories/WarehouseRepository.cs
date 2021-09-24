using MagazynApp.Data.Interfaces;
using MagazynApp.Exceptions;
using MagazynApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace MagazynApp.Data.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly MagazynContext _context;

        public WarehouseRepository(MagazynContext context)
        {
            _context = context;
        }

        public async Task<List<Sector>> SectorsToList()
        {
            return await _context.Sector
                .Include(s => s.Alleys)
                .ThenInclude(s => s.Shelves)
                .ThenInclude(s => s.Sockets)
                .ToListAsync();
        }

        public async Task<List<Alley>> AlleysToList()
        {
            return await _context.Alley
                .Include(a => a.Sector)
                .Include(a => a.Shelves)
                .ThenInclude(a => a.Sockets)
                .ToListAsync();
        }

        public async Task<List<Shelf>> ShelvesToList()
        {
            return await _context.Shelf
                .Include(s => s.Alley)
                .ThenInclude(s => s.Sector)
                .Include(s => s.Sockets)
                .ToListAsync();
        }

        public async Task<List<Socket>> SocketsToList()
        {
            return await _context.Socket
                .Include(s => s.SocketProduct)
                .ThenInclude(s => s.Product)
                .Include(s => s.Shelf)
                .ThenInclude(s => s.Alley)
                .ThenInclude(s => s.Sector)
                .ToListAsync();
        }

        public async Task<Sector> FindSector(int sectorId)
        {
            return await _context.Sector
                .Include(s => s.Alleys)
                .ThenInclude(s => s.Shelves)
                .ThenInclude(s => s.Sockets)
                .FirstOrDefaultAsync(s => s.Id == sectorId);
        }

        public async Task<Alley> FindAlley(int alleyId)
        {
            return await _context.Alley
                .Include(a => a.Sector)
                .Include(a => a.Shelves)
                .ThenInclude(a => a.Sockets)
                .FirstOrDefaultAsync(a => a.Id == alleyId);
        }

        public async Task<Shelf> FindShelf(int shelfId)
        {
            return await _context.Shelf
                .Include(s => s.Alley)
                .ThenInclude(s => s.Sector)
                .Include(s => s.Sockets)
                .FirstOrDefaultAsync(s => s.Id == shelfId);
        }

        public async Task<Socket> FindSocket(int socketId)
        {
            return await _context.Socket
                .Include(s => s.Shelf)
                .ThenInclude(s => s.Alley)
                .ThenInclude(s => s.Sector)
                .FirstOrDefaultAsync(s => s.Id == socketId);
        }

        public async Task<List<SocketProduct>> SocketProductToList()
        {
            return await _context.SocketProduct
                .Include(s => s.Product)
                .Include(s => s.Socket)
                .ThenInclude(s => s.Shelf)
                .ThenInclude(s => s.Alley)
                .ThenInclude(s => s.Sector)
                .ToListAsync();
        }
        

        public async Task AddShelf(Shelf shelf)
        {
            await _context.AddAsync(shelf);
            await _context.SaveChangesAsync();
        }

        public async Task AddSocketRange(IEnumerable<Socket> sockets)
        {
            await _context.AddRangeAsync(sockets);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Socket>> CreateSockets(int numberOfSockets, int maxCapacity, int shelfId)
        {
            var sockets = new List<Socket>();

            for (var i = 0; i < numberOfSockets; i++)
            {
                var socket = new Socket
                {
                    Name = 'S' + (i + 1).ToString(),
                    Capacity = 0,
                    MaxCapacity = maxCapacity,
                    ShelfId = shelfId
                };
                sockets.Add(socket);
            }

            return sockets;
        }

        public async Task AddSector(Sector sector)
        {
            await _context.AddAsync(sector);
            await _context.SaveChangesAsync();
        }

        public async Task<Sector> CreateSector(string name)
        {
            var sector = new Sector
            {
                Name = name
            };
            return sector;
        }

        public async Task<Alley> CreateAlley(int sectorId, string name)
        {
            var alley = new Alley
            {
                SectorId = sectorId,
                Name = name
            };
            return alley;
        }

        public async Task AddAlley(Alley alley)
        {
            await _context.AddAsync(alley);
            await _context.SaveChangesAsync();
        }
    }
}