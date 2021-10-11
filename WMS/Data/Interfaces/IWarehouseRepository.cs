using MagazynApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<List<Sector>> SectorsToList();
        Task<List<Alley>> AlleysToList();
        Task<List<Alley>> AlleysBySectorId(int sectorId);
        Task<List<Shelf>> ShelvesToList();
        Task<List<Socket>> SocketsToList();
        Task<Sector> FindSector(int sectorId);
        Task<Alley> FindAlley(int alleyId);
        Task<Shelf> FindShelf(int shelfId);
        Task<Socket> FindSocket(int socketId);
        Task <List<SocketProduct>> SocketProductToList();
        Task <List<SocketProduct>> SocketProductBySocketId(int socketId);
        Task AddShelf(Shelf shelf);
        Task AddSocketRange(IEnumerable<Socket> sockets);
        Task<IEnumerable<Socket>> CreateSockets(int numberOfSockets, int maxCapacity, int shelfId);
        Task AddSector(Sector sector);
        Task<Sector> CreateSector(string name);
        Task<Alley> CreateAlley(int sectorId, string name);
        Task AddAlley(Alley alley);
        Task<double> WarehouseOccupiedSpace();
        Task<List<Socket>> InCompleteSockets();

        Task MovingProductsFromSocket(List<SocketProduct> socketProducts,
            List<Socket> selectedSockets);


    }
}