using System.Linq;
using System.Threading.Tasks;
using MagazynApp.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MagazynApp.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseController(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }
        // GET
        public async Task<IActionResult> Sectors()
        {
            return View(await _warehouseRepository.SectorsToList());
        }

        public async Task<IActionResult> Alleys(int sectorId)
        {
            //return View(await _warehouseRepository.SectorAlleys(sectorId));
            return View( _warehouseRepository.FindSector(sectorId).Result.Alleys.ToList());
        }

        public async Task<IActionResult> Shelves(int alleyId)
        {
            return View(_warehouseRepository.FindAlley(alleyId).Result.Shelves.ToList());
        }

        public async Task<IActionResult> Sockets(int shelfId)
        {
            return View(_warehouseRepository.FindShelf(shelfId).Result.Sockets.ToList());
        }

        public async Task<IActionResult> SocketDetails(int socketId)
        {
            var socket = _warehouseRepository.FindSocket(socketId);
            return View();
        }
    }
}