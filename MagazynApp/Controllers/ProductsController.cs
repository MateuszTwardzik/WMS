using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagazynApp.Data;
using MagazynApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using MagazynApp.Data.Interfaces;

namespace MagazynApp.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;

        public ProductsController(IProductRepository productRepository, IProductTypeRepository productTypeRepository)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
        }
        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        public async Task<IActionResult> Index(string searchString, int? pageNumber, string currentFilter,
            string sortOrder, int pageSize, string setPageSize)
        {
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["TypeSortParm"] = sortOrder == "type" ? "type_desc" : "type";
            ViewData["QuantitySortParm"] = sortOrder == "quantity" ? "quantity_desc" : "quantity";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "price_desc" : "price";
            ViewData["CurrentFilter"] = searchString;

            var products = _productRepository.GetProducts();
            if (sortOrder != null)
            {
                HttpContext.Session.SetString("sort", sortOrder);
            }
            sortOrder = HttpContext.Session.GetString("sort");

            switch (sortOrder)
            {
                case "name":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "quantity":
                    products = products.OrderBy(p => p.Quantity);
                    break;
                case "quantity_desc":
                    products = products.OrderByDescending(p => p.Quantity);
                    break;
                case "price":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "type":
                    products = products.OrderBy(p => p.Type.Name);
                    break;
                case "type_desc":
                    products = products.OrderByDescending(p => p.Type.Name);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            switch (setPageSize)
            {
                case "set10":
                    HttpContext.Session.SetInt32("pagesize", 10);
                    break;
                case "set20":
                    HttpContext.Session.SetInt32("pagesize", 20);
                    break;
                case "set100":
                    HttpContext.Session.SetInt32("pagesize", 100);
                    break;
                case "setAll":
                    HttpContext.Session.SetInt32("pagesize", products.Count());
                    break;
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.ToUpper().Contains(searchString.ToUpper()));
            }


            if (HttpContext.Session.GetInt32("pagesize").HasValue)
            {
                pageSize = HttpContext.Session.GetInt32("pagesize").GetValueOrDefault();
            }
            else pageSize = 10;

            return View(await PaginatedList<Product>.CreateAsyc(products, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _productRepository.FindProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        public IActionResult Create()
        {
            var typeList = _productTypeRepository.ProductTypesToList();
            ViewData["productTypes"] = typeList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Quantity,Price,TypeId")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.FindProductByIdAsync(id);
            var typeList = _productTypeRepository.ProductTypesToList();

            ViewData["productTypes"] = typeList;
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Quantity,Price,TypeId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.UpdateProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_productRepository.ProductExists(product.Id))
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
            return View(product);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _productRepository.FindProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
