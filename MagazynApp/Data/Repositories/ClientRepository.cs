using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly MagazynContext _context;
        public ClientRepository(MagazynContext context)
        {
            _context = context;
        }
        public async Task<List<Client>> ClientsToListAsync()
        {
            return await _context.Client.ToListAsync();
        }
        public async Task<Client> FindClientByIdAsync(int? id)
        {
            return await _context.Client.FirstOrDefaultAsync(m => m.Id == id); ;
        }
        public async Task AddClientAsync(Client client)
        {
            _context.Add(client);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteClientAync(int id)
        {
            var client = await _context.Client.FindAsync(id);
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateClientAsync(Client client)
        {
            _context.Update(client);
            await _context.SaveChangesAsync();
        }
        public bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.Id == id);
        }
    }
}
