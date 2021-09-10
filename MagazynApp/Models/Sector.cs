using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class Sector
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double Capacity
        {
            get { return Alleys.Sum(s => s.Capacity); }
        }

        public double MaxCapacity
        {
            get { return Alleys.Sum(s => s.MaxCapacity); }
        }

        public IList<Alley> Alleys { get; set; }
        //  public IList<ProductType> AllowedTypesOfProducts;
    }
}