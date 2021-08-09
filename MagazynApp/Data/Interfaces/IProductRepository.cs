using MagazynApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Interfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProducts();
        Task<Product> FindProductByIdAsync(int? id);
        Task AddProductAsync(Product product);
        Task DeleteProduct(int id);
        Task UpdateProduct(Product product);
        bool ProductExists(int id);
    }
}
