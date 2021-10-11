using MagazynApp.Data.Interfaces;
using MagazynApp.Models;
using MagazynApp.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Repositories
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly MagazynContext _context;

        public ProductTypeRepository(MagazynContext context)
        {
            _context = context;
        }
        public IList<ProductType> ProductTypesToList()
        {
            var types = _context.ProductType;

            IList<ProductType> typeList = new List<ProductType>();

            foreach (var t in types)
            {
                typeList.Add(t);
            }
            return typeList;
        }
        public async Task<IList<ProductTypeViewModel>> ProductTypesAsyncToListWithAmount()
        {
            var types = await _context.ProductType
                    .Include(t => t.Product)
                    .Select(t => new ProductTypeViewModel()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Quantity = t.Product.Count
                    })
                    .ToListAsync();

            return types;
        }
        public async Task<ProductType> FindProductTypeByIdAsync(int? id)
        {
            var productType = await _context.ProductType
                .FirstOrDefaultAsync(m => m.Id == id);
            return productType;
        }
        public async Task AddProductTypeAsync(ProductType productType)
        {
            _context.Add(productType);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProductType(int id)
        {
            var productType = await _context.ProductType.FindAsync(id);
            _context.ProductType.Remove(productType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductType(ProductType productType)
        {
            _context.Update(productType);
            await _context.SaveChangesAsync();
        }
        public bool ProductTypeExists(int id)
        {
            return _context.ProductType.Any(e => e.Id == id);
        }
    }
}
