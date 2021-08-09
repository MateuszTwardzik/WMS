using MagazynApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Interfaces
{
    public interface IClientRepository
    {
        Task<List<Client>> ClientsToListAsync();
        Task<Client> FindClientByIdAsync(int? id);
        Task AddClientAsync(Client client);
        Task DeleteClientAync(int id);
        Task UpdateClientAsync(Client client);
        bool ClientExists(int id);
    }
}
