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
            var products = _context.Product
               .Include(p => p.Type);

            return products;
        }
        public async Task<Product> FindProductByIdAsync(int? id)
        {
            var products = _context.Product
                 .Include(p => p.Type);

            var product = await products.FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }
        public async Task AddProductAsync(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
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
    }
}
