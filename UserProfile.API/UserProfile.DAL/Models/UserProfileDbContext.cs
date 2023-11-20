using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UserProfile.DAL.Models
{
    public class UserProfileDbContext: IdentityDbContext
    {
        public UserProfileDbContext():base()
        {
            
        }
        public UserProfileDbContext(DbContextOptions<UserProfileDbContext> options) : base(options) 
        {
        }
        public DbSet<User> Users{ get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-N86E7N8\\SQLEXPRESS;Initial Catalog=dbUserProfile;Integrated Security=True;TrustServerCertificate=True;Encrypt=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.Uid);
            base.OnModelCreating(modelBuilder);
            SeedData_Roles(modelBuilder);
        }
        public void SeedData_Roles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                 new IdentityRole {Name = "Admin", NormalizedName = "ADMIN"},
                 new IdentityRole {Name = "User", NormalizedName = "USER"}
            );
        }
    }
}


