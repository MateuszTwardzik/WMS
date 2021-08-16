using MagazynApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    
    public class MissingOrderedProduct
    {
        //private readonly IProductRepository _productRepository;
        //private MissingOrderedProduct(IProductRepository productRepository)
        //{
        //    _productRepository = productRepository;
        //}

        //public MissingOrderedProduct()
        //{

        //}
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int ProductAmount { get; set; }
        public int OrderedAmount { get; set; }
        public int MissingAmount
        {
            get
            {
                return OrderedAmount - ProductAmount;
            }
        }

    }
}
