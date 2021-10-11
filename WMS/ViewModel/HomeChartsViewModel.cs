using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.ViewModel
{
    public class HomeChartsViewModel
    {
        public IEnumerable<ProductsChartViewModel> ProductTypesPieChart;
        public IEnumerable<OrderLineChartViewModel> OrderLineChart;
    }
}
