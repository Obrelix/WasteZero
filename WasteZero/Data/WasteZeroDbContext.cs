using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WasteZero.Models;

namespace WasteZero.Data
{
    public class WasteZeroDbContext : DbContext {
        protected readonly IConfiguration Configuration;
        public WasteZeroDbContext(IConfiguration configuration) {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("ProjectDB"));
        }
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductDetail> ProductDetails { get; set; } = null!;
        public virtual DbSet<ProductType> ProductTypes { get; set; } = null!;
        public virtual DbSet<ConsumedDetail> ConsumedDetails { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Product>()
                .ToTable("Products");
            modelBuilder.Entity<Product>(entity => {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Name).HasMaxLength(250);
            });
            modelBuilder.Entity<ProductDetail>()
                .ToTable("ProductDetails");
            modelBuilder.Entity<ProductDetail>(entity => {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<ProductType>()
                .ToTable("ProductTypes");
            modelBuilder.Entity<ProductType>(entity => {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Code).HasMaxLength(5);
            });
            modelBuilder.Entity<ConsumedDetail>()
                .ToTable("ConsumedDetails");
            modelBuilder.Entity<ConsumedDetail>(entity => {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");
            });
        }

    }
}
