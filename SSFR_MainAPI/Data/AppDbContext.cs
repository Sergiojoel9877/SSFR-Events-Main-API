using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSFR_MainAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<Applicant> Applicants { get; set; }
        //public DbSet<Deposit> Deposits { get; set; }
        //public DbSet<Location> Locations { get; set; }
        //public DbSet<Operation> Operations { get; set; }
        //public DbSet<Operant> Operants { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<Pallet> Pallets { get; set; }
        //public DbSet<Picking> Pickings { get; set; }
        //public DbSet<Printer> Printers { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<Serial> Serials { get; set; }
        //public DbSet<Shelf> Shelves { get; set; }
        //public DbSet<Status> Statuses { get; set; }
        //public DbSet<Stock> Stocks { get; set; }
        //public DbSet<Tramer> Tramers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)   
        {
            base.OnModelCreating(builder);
        }
    }
}
