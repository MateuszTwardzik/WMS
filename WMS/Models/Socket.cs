using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class Socket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName => Shelf.Alley.Sector.Name + " " + Shelf.Alley.Name + " " + Shelf.Name + " " + Name;
        public double Capacity { get; set; }
        public double MaxCapacity { get; set; } //160 == 8 kartonow (1 paleta)
        public int ShelfId { get; set; }
        public Shelf Shelf { get; set; }
        
        public ICollection<SocketProduct> SocketProduct { get; set; }
    }
}
