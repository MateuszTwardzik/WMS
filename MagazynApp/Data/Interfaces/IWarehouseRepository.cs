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
        Task<List<Shelf>> ShelvesToList();
        Task<List<Socket>> SocketsToList();
        
        Task<Sector> FindSector(int sectorId);
        // Task<Sector> FindSector(Sector alley);
        // Task<Sector> FindSector(Shelf shelf);
        // Task<Sector> FindSector(Socket socket);

        Task<Alley> FindAlley(int alleyId);
        // Task<Alley> FindAlley(Shelf shelf);
        // Task<Alley> FindAlley(Socket socket);
        
        Task<Shelf> FindShelf(int shelfId);
        // Task<Shelf> FindShelf(Socket socket);

        Task<Socket> FindSocket(int socketId);



    }
}