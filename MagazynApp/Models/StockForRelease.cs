using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class StockForRelease
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int StateId { get; set; }
        public StockForReleaseState State { get; set; }
    }
}
