using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using MagazynApp.ViewModel;
using Microsoft.AspNetCore.Http;
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
            return View(sectorId == 0
                ? await _warehouseRepository.AlleysToList()
                : _warehouseRepository.FindSector(sectorId).Result.Alleys.ToList());
        }

        public async Task<IActionResult> Shelves(int alleyId)
        {
            return View(alleyId == 0
                ? await _warehouseRepository.ShelvesToList()
                : _warehouseRepository.FindAlley(alleyId).Result.Shelves.ToList());
        }

        public async Task<IActionResult> Sockets(int shelfId)
        {
            return View(shelfId == 0
                ? await _warehouseRepository.SocketsToList()
                : _warehouseRepository.FindShelf(shelfId).Result.Sockets.ToList());
        }

        public async Task<IActionResult> SocketDetails(int socketId)
        {
            var sockets = _warehouseRepository.SocketProductToList().Result.Where(s => s.SocketId == socketId).ToList();

            var socketProducts = sockets.Select(s => new SocketProductViewModel()
            {
                ProductName = s.Product.Name,
                ProductAmount = s.Amount
            }).ToList();

            var socket = await _warehouseRepository.FindSocket(socketId);

            var socketsViewModel = new SocketViewModel()
            {
                SocketName = socket.Name,
                Capacity = socket.Capacity,
                MaxCapacity = socket.MaxCapacity,
                Products = socketProducts
            };

            return View(socketsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateShelf()
        {
            var sectors = await _warehouseRepository.SectorsToList();
            ViewBag.SectorList = sectors;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShelf(int alleyId, string Name, int numberOfSockets,
            int socketMaxCapacity)
        {
            try
            {
                var shelf = new Shelf()
                {
                    Name = Name,
                    AlleyId = alleyId,
                    Alley = await _warehouseRepository.FindAlley(alleyId),
                };
                await _warehouseRepository.AddShelf(shelf);

                var sockets = await _warehouseRepository.CreateSockets(numberOfSockets, socketMaxCapacity, shelf.Id);

                await _warehouseRepository.AddSocketRange(sockets);
                return RedirectToAction("Shelves");
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAlleysBySectorId(int sectorId)
        {
            var alleys = await _warehouseRepository.AlleysToList();
            var alleysBySector = alleys.Where(a => a.SectorId == sectorId).ToList();
            var data = alleysBySector.Select(a => new AlleyViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                SectorId = a.SectorId
            }).ToList();
            return Json(data);
        }

        [HttpGet]
        public IActionResult CreateSector()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSector(string Name)
        {
            var sector = await _warehouseRepository.CreateSector(Name);
            await _warehouseRepository.AddSector(sector);
            return RedirectToAction("Sectors");
        }

        [HttpGet]
        public async Task<IActionResult> CreateAlley()
        {
            var sectors = await _warehouseRepository.SectorsToList();
            ViewBag.SectorList = sectors;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlley(int sectorId, string Name)
        {
            var alley = await _warehouseRepository.CreateAlley(sectorId, Name);
            await _warehouseRepository.AddAlley(alley);
            return RedirectToAction("Alleys");
        }
        [HttpGet]
        public async Task<IActionResult> ProductsLocation()
        {
            var socketProducts = await _warehouseRepository.SocketProductToList();
            
            var productsList = socketProducts
                .GroupBy(p => p.Product.Name)
                .Select(p => new ProductLocationViewModel
                {
                    Name = p.Key,
                    Amount = p.Sum(p => p.Amount),
                    Sockets = p.Select(p => p.Socket)
                   // Sockets = p.Select(p => p.Socket.FullName)
                    
                })
                .OrderBy(p => p.Name)
                .ToList();

            return View(productsList);
        }
    }
}