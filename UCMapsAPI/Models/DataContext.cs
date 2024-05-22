using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCMapsAPI.Models;

namespace UCMapsAPI.Controllers
{
    public partial class SampleDBContext : DbContext
    {
        public SampleDBContext(DbContextOptions
        <SampleDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Marker> Marker { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TokenInfo> TokenInfo { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Marker>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}