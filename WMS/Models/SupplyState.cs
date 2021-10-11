using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class SupplyState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Supply> Supply { get; set; }
    }
}
