using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class StockForReleaseState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<StockForRelease> StockForRelease { get; set; }
    }
}
