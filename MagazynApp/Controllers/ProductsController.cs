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

namespace MagazynApp.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly MagazynContext _context;

        public ProductsController(MagazynContext context)
        {
            _context = context;
        }
        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString, int? pageNumber, string currentFilter,
            string sortOrder, int pageSize, string setPageSize)
        {
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["TypeSortParm"] = sortOrder == "type" ? "type_desc" : "type";
            ViewData["QuantitySortParm"] = sortOrder == "quantity" ? "quantity_desc" : "quantity";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "price_desc" : "price";
            ViewData["CurrentFilter"] = searchString;

                var products = _context.Product
                               .Include(p => p.Type)
                               .AsNoTracking();

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

                return View(await PaginatedList<Product>.CreateAsyc(products.AsNoTracking(), pageNumber ?? 1, pageSize));
            }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(t => t.Type)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("userId") != null)
            {
                var types = _context.ProductType;

                IList<ProductType> typeList = new List<ProductType>();

                foreach (var t in types)
                {
                    typeList.Add(t);
                }

                ViewData["productTypes"] = typeList;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }


        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Quantity,Price,TypeId")] Product product)
        //[Bind("Id,Name,Quantity,Price")] 
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);

            var types = _context.ProductType;

            IList<ProductType> typeList = new List<ProductType>();

            foreach (var t in types)
            {
                typeList.Add(t);
            }

            ViewData["productTypes"] = typeList;
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
