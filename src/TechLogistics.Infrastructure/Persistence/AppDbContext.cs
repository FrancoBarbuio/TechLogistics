using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TechLogistics.Domain.Entities;


namespace TechLogistics.Infrastructure.Persistence
{
    // IMPORTANTE: Debe ser 'public'
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Shipping> Shippings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipping>().HasKey(s => s.Id);
            modelBuilder.Entity<Shipping>()
                .Property(s => s.TrackingNumber)
                .IsRequired()
                .HasMaxLength(20);

            base.OnModelCreating(modelBuilder);
        }
    }
}