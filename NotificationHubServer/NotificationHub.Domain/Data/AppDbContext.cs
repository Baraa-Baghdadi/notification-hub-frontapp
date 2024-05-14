using Microsoft.EntityFrameworkCore;
using NotificationHub.Domain.Models;
using NotificationHub.Domain.Models.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Domain.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> products { get; set; }
        public DbSet<SignalRUser> signalRUsers { get; set; }
        public DbSet<Notification> notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<SignalRUser>(b =>
            {
                b.ToTable("signalRUsers");
            });

            modelBuilder.Entity<Notification>().ToTable("notifications");

        }
    }
}
