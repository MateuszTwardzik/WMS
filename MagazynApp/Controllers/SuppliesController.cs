using MagazynApp.Data;
using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagazynApp.Exceptions;

namespace MagazynApp.Controllers
{
    [Authorize]
    public class SuppliesController : Controller
    {
        private readonly ISupplyRepository _supplyRepository;

        public SuppliesController(ISupplyRepository supplyRepository)
        {
            _supplyRepository = supplyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _supplyRepository.SupplyToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupply(int productId, int amount)
        {
            await _supplyRepository.CreateSupplyAsync(productId, amount);
            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        public async Task<IActionResult> CompleteSupply(int supplyId)
        {
            try
            {
                var supply = await _supplyRepository.FindSupplyAsync(supplyId);
                await _supplyRepository.CompleteSupplyAsync(supply);
            }
            catch (SocketNotFoundException)
            {
                TempData["error"] = "Brak miejsca w magazynie";
                
            }
            catch (SupplyIsCompletedException)
            {
                TempData["error"] = "Zamówienie zostało już zrealizowane";
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSupply(int supplyId)
        {
            await _supplyRepository.DeleteSupplyAsync(supplyId);
            return RedirectToAction("Index");
        }
    }
}