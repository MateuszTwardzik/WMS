using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagazynApp.Models;

namespace MagazynApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(MagazynContext context)
        {
            context.Database.EnsureCreated();

            var products = new Product[]
            {
               // new Product{Name="Cos", Quantity=1,Price=4}
            };
            foreach(Product p in products)
            {
                context.Add(p);
            }
            context.SaveChanges();

        }
    }
}
