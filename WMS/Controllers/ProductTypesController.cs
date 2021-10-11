using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagazynApp.Data;
using MagazynApp.Models;
using MagazynApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using MagazynApp.Data.Interfaces;

namespace MagazynApp.Controllers
{
    [Authorize (Roles = "Admin, User")]
    public class ProductTypesController : Controller
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductRepository _productRepository;

        public ProductTypesController(IProductTypeRepository productTypeRepository, IProductRepository productRepository)
        {
            _productTypeRepository = productTypeRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var types = await _productTypeRepository.ProductTypesAsyncToListWithAmount();

            return View(types);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = await _productTypeRepository.FindProductTypeByIdAsync(id);
            if (productType == null)
            {
                return NotFound();
            }
            ViewData["products"]  = await _productRepository.ProductsByType(productType.Id);
            
            return View(productType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ProductType productType)
        {
            if (ModelState.IsValid)
            {
                await _productTypeRepository.AddProductTypeAsync(productType);
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _productTypeRepository.FindProductTypeByIdAsync(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ProductType productType)
        {
            if (id != productType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productTypeRepository.UpdateProductType(productType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_productTypeRepository.ProductTypeExists(productType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = await _productTypeRepository.FindProductTypeByIdAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productTypeRepository.DeleteProductType(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
