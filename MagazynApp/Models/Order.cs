using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class Order
    {
        [Column("OrderId")]
        public int Id { get; set; }
       // public int ProductId { get; set; }
        //public int OrderDetailId { get; set; }
        public IList<OrderDetail> OrderLines { get; set; } = new List<OrderDetail>();
      //  public string FirstName { get; set; }
       // public string LastName { get; set; }


        //public Product Product { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int StateId { get; set; }
        public OrderState State { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }


    }
}
