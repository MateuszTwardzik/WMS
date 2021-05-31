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
            if (context.User.Any())
            {
                return;
            }

            var users = new User[]
            {
                new User{Name="admin", Password="admin", Permission=0}
            };
            foreach (User u in users)
            {
                context.User.Add(u);
            }
            context.SaveChanges();

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
