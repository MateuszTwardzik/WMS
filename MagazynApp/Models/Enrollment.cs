using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public Product Order { get; set; }
        public ProductType Product { get; set; }
    }
}
