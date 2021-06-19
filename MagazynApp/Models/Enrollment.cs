using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int ProductID { get; set; }
        public int Product_TypeID { get; set; }

        public Product Product { get; set; }
        public ProductType Type { get; set; }
    }
}
