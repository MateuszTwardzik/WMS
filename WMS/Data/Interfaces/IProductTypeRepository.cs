using MagazynApp.Models;
using MagazynApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Data.Interfaces
{
    public interface IProductTypeRepository
    {
        IList<ProductType> ProductTypesToList();
        Task<IList<ProductTypeViewModel>> ProductTypesAsyncToListWithAmount();
        Task<ProductType> FindProductTypeByIdAsync(int? id);
        Task AddProductTypeAsync(ProductType productType);
        Task DeleteProductType(int id);
        Task UpdateProductType(ProductType productType);
        bool ProductTypeExists(int id);
    }
}
