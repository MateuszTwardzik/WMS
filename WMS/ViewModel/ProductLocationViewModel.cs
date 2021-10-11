using System.Collections.Generic;
using MagazynApp.Models;

namespace MagazynApp.ViewModel
{
    public class ProductLocationViewModel
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public IEnumerable<Socket> Sockets { get; set; }
       // public IEnumerable<string> Sockets { get; set; }
    }
}