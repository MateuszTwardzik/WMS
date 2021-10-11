using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using MagazynApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace MagazynApp.Controllers
{
    [Authorize(Roles = "Admin, User")]
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
                : (await _warehouseRepository.FindSector(sectorId)).Alleys.ToList());
        }

        public async Task<IActionResult> Shelves(int alleyId)
        {
            return View(alleyId == 0
                ? await _warehouseRepository.ShelvesToList()
                : (await _warehouseRepository.FindAlley(alleyId)).Shelves.ToList());
        }

        public async Task<IActionResult> Sockets(int shelfId)
        {
            return View(shelfId == 0
                ? await _warehouseRepository.SocketsToList()
                : (await _warehouseRepository.FindShelf(shelfId)).Sockets.ToList());
        }

        public async Task<IActionResult> SocketDetails(int socketId)
        {
            var sockets = await _warehouseRepository.SocketProductBySocketId(socketId);
            // var sockets = (await _warehouseRepository.SocketProductToList()).Where(s => s.SocketId == socketId).ToList();

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
            //var alleys = await _warehouseRepository.AlleysToList();
            //var alleysBySector = alleys.Where(a => a.SectorId == sectorId).ToList();
            var alleysBySector = await _warehouseRepository.AlleysBySectorId(sectorId);
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
                })
                .OrderBy(p => p.Name)
                .ToList();

            return View(productsList);
        }

        [HttpGet]
        public async Task<IActionResult> MovingProductsFromSocket(int socketId)
        {
            var socket = await _warehouseRepository.FindSocket(socketId);
            var socketProduct = await _warehouseRepository.SocketProductBySocketId(socketId);
            var sockets = await _warehouseRepository.InCompleteSockets();
            var movingSocket = new MovingSocketViewModel
            {
                Socket = socket,
                SocketProducts = socketProduct,
                InCompleteSockets = sockets
            };
            return View(movingSocket);
        }

        [HttpGet]
        public async Task<ActionResult> Test()
        {
            var sockets = await _warehouseRepository.InCompleteSockets();
            var socketViewModel = sockets.Select(s => new SocketViewModel
            {
                SocketName = s.FullName,
                Capacity = s.Capacity,
                MaxCapacity = s.MaxCapacity
            });
            //return Json(new {data = await _warehouseRepository.InCompleteSockets()});
            return Json(new {data = socketViewModel});
        }

        [HttpPost]
        public async Task<IActionResult> MovingProductsFromSocket(List<SocketProduct> socketProducts,
            List<Socket> selectedSockets)
        {
            if (socketProducts.Sum(s => s.Amount) <= selectedSockets.Sum(s => s.MaxCapacity - s.Capacity))
            {
                await _warehouseRepository.MovingProductsFromSocket(socketProducts, selectedSockets);
            }
            else
            {
                TempData["Error"] = "Produkty nie zmieszcza sie w wybranych gniazdach";
            }

            return View();
        }
    }
}