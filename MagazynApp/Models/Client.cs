using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        [DataType(DataType.PostalCode)]
        [Column("Postal_Code")]
        public string PostalCode { get; set; }
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public ICollection<Order> Order { get; set; }


    }
}
