using itstepimagesproject.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Services
{
    public class ResourcesDbContext : DbContext
    {
        public ResourcesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Profile>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Profile>()
                .Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Profile>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
