using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MagazynContext _context;

        public ProductRepository(MagazynContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetProducts()
        {
            return _context.Product
                .Include(p => p.Type);
        }

        public async Task<Product> FindProductByIdAsync(int? id)
        {
            // var products = _context.Product
            //     .Include(p => p.Type);
            //
            // var product = await products.FirstOrDefaultAsync(p => p.Id == id);
           // return product;
           return await _context.Product
               .Include(p => p.Type)
               .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await FindProductByIdAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }

        public bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        public async Task SetAmountAsync(int id, int amount)
        {
            var product = await FindProductByIdAsync(id);
            product.Quantity = amount;
            await UpdateProduct(product);
        }

        public Task<List<Product>> ProductsToListAsync()
        {
            return _context.Product
                .Include(p => p.Type)
                .ToListAsync();
        }

        public async Task<List<Product>> ProductsByType(int typeId)
        {
            return await _context.Product
                .Include(p => p.Type)
                .Where(p => p.TypeId == typeId)
                .ToListAsync();
        }
    }
}