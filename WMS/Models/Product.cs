using Castle.MicroKernel.SubSystems.Conversion;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagazynApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true)]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int TypeId { get; set; }
        public ProductType Type { get; set; }
        public ICollection<OrderDetail> OrderLines { get; set; }
        public virtual ICollection<SocketProduct> SocketProduct { get; set; }
    }
}