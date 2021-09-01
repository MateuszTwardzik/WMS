using MagazynApp.Data;
using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Controllers
{
    public class SuppliesController : Controller
    {

        private readonly MagazynContext _context;
        private readonly IProductRepository _productRepository;

        public SuppliesController(MagazynContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var suplies = await _context.Supply.Include(s => s.State).Include(s => s.Product).ToListAsync();
            return View(suplies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupply(int productId, int amount)
        {
            var supply = new Supply
            {
                ProductId = productId,
                Amount = amount,
                StateId = 1,
                OrderDate = DateTime.Now

            };
            await _context.AddAsync(supply);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CompleteSupply(int? supplyId)
        {
            var supply = await _context.Supply.Include(s => s.State).Include(s => s.Product).FirstOrDefaultAsync(s => s.Id == supplyId);
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                supply.StateId = 2;
                supply.CompletionDate = DateTime.Now;
                supply.Product.Quantity += supply.Amount;
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();

            }
            return RedirectToAction("Index");
        }


    }
}
