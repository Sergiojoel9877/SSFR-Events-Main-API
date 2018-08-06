using Microsoft.EntityFrameworkCore;
using SSFR_MainAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSFR_MainAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<UserSignUp> UserSignUps { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)   
        {
            base.OnModelCreating(builder);
        }
    }
}
