using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class Supply
    {
        public int Id { get; set; }
        public IList<SupplyDetail> SupplyDetails { get; set; } = new List<SupplyDetail>();
        public int StateId { get; set; }
        public SupplyState State { get; set; }
       // public int Amount { get; set; }
    }
}
