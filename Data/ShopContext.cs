using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NuntaNoastra_Buta_Camelia.Models;

namespace NuntaNoastra_Buta_Camelia.Data
{
    public class ShopContext:DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) :
       base(options)
        { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Candle> Candles { get; set; }
        public DbSet<Distribuitor> Distribuitors { get; set; }
        public DbSet<DistribuitorCandle> DistribuitorCandles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          modelBuilder.Entity<Customer>().ToTable("Customer");
          modelBuilder.Entity<Order>().ToTable("Order");
          modelBuilder.Entity<Candle>().ToTable("Candle");
         
          modelBuilder.Entity<Distribuitor>().ToTable("Distribuitor");
          modelBuilder.Entity<DistribuitorCandle>().ToTable("DistribuitorCandle");
          modelBuilder.Entity<DistribuitorCandle>()
          .HasKey(c => new { c.CandleID, c.DistribuitorID });//configureaza cheia primara compusa;
        }
    }
}
