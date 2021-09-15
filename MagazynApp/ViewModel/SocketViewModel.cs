using System.Collections.Generic;

namespace MagazynApp.ViewModel
{
    public class SocketViewModel
    {
        public string SocketName { get; set; }
        public double Capacity { get; set; }
        public double MaxCapacity { get; set; }
        public List<SocketProductViewModel> Products { get; set; }
    }
}