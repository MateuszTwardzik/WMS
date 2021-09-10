using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class Alley
    {
        public int Id { get; set; }
        public int SectorId { get; set; }
        public Sector Sector { get; set; }
        public string Name { get; set; }

        public double Capacity
        {
            get { return Shelves.Sum(s => s.Capacity); }
        }

        public double MaxCapacity
        {
            get { return Shelves.Sum(s => s.MaxCapacity); }
        }

        public IList<Shelf> Shelves { get; set; }
    }
}