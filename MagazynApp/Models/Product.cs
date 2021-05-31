using Castle.MicroKernel.SubSystems.Conversion;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagazynApp.Models
{
    public class Product
    {        
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public decimal Quantity { get; set; }
        //[DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }   

    }
}
