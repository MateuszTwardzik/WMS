using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.ViewModel
{
    public class StackedProductsChartViewModel
    {
        public string StackedDimensionOne { get; set; }
        public List<ProductsChartViewModel> LstData { get; set; }
    }
}
