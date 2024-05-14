using Microsoft.EntityFrameworkCore;
using NotificationHub.Domain.Data;
using NotificationHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationHub.Domain.Seeding
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {

            if (!context.products.Any())
            {
                var products = new List<Product>
                {
                    new Product("P01","product1","desc1",10,10),
                    new Product("P02","product2","desc2",10,10),
                }; 
                context.products.AddRange(products);
            }


            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
