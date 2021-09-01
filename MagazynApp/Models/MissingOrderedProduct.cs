using MagazynApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    
    public class MissingOrderedProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
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
