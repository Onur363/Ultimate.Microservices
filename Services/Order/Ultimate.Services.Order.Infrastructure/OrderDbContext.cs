using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Services.Order.Domain.OrderAggreate;

namespace Ultimate.Services.Order.Infrastructure
{
    public class OrderDbContext:DbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";
        public OrderDbContext(DbContextOptions<OrderDbContext> options):base(options)
        {

        }

        public DbSet<Domain.OrderAggreate.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.OrderAggreate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
            modelBuilder.Entity<Domain.OrderAggreate.OrderItem>().ToTable("OrderItems", DEFAULT_SCHEMA);

            modelBuilder.Entity<OrderItem>().Property(x => x.Price).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Domain.OrderAggreate.Order>().OwnsOne(x => x.Address).WithOwner();
            base.OnModelCreating(modelBuilder);
        }
    }
}
