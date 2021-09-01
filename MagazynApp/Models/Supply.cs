using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class Supply
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
        public int StateId { get; set; }
        public SupplyState State { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime CompletionDate { get; set; }
        // public int Amount { get; set; }
    }
}
