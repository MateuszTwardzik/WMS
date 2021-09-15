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
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly MagazynContext _context;

        public WarehouseRepository(MagazynContext context)
        {
            _context = context;
        }

        public Task<List<Sector>> SectorsToList()
        {
            return _context.Sector
                .Include(s => s.Alleys)
                .ThenInclude(s => s.Shelves)
                .ThenInclude(s => s.Sockets)
                .ToListAsync();
        }

        public Task<List<Alley>> AlleysToList()
        {
            return _context.Alley
                .Include(a => a.Sector)
                .Include(a => a.Shelves)
                .ThenInclude(a => a.Sockets)
                .ToListAsync();
        }

        public Task<List<Shelf>> ShelvesToList()
        {
            return _context.Shelf
                .Include(s => s.Alley)
                .ThenInclude(s => s.Sector)
                .Include(s => s.Sockets)
                .ToListAsync();
        }

        public Task<List<Socket>> SocketsToList()
        {
            return _context.Socket
                .Include(s => s.Shelf)
                .ThenInclude(s => s.Alley)
                .ThenInclude(s => s.Sector)
                .ToListAsync();
        }

        public Task<Sector> FindSector(int sectorId)
        {
            return _context.Sector
                .Include(s => s.Alleys)
                .ThenInclude(s => s.Shelves)
                .ThenInclude(s => s.Sockets)
                .FirstOrDefaultAsync(s => s.Id == sectorId);
        }

        public Task<Alley> FindAlley(int alleyId)
        {
            return _context.Alley
                .Include(a => a.Sector)
                .Include(a => a.Shelves)
                .ThenInclude(a => a.Sockets)
                .FirstOrDefaultAsync(a => a.Id == alleyId);
        }

        public Task<Shelf> FindShelf(int shelfId)
        {
            return _context.Shelf
                .Include(s => s.Alley)
                .ThenInclude(s => s.Sector)
                .Include(s => s.Sockets)
                .FirstOrDefaultAsync(s => s.Id == shelfId);
        }

        public Task<Socket> FindSocket(int socketId)
        {
            return _context.Socket
                .Include(s => s.Shelf)
                .ThenInclude(s => s.Alley)
                .ThenInclude(s => s.Sector)
                .FirstOrDefaultAsync(s => s.Id == socketId);
        }


    }
}