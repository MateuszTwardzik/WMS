using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class OrderState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
