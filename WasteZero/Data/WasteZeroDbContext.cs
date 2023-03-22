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
        public virtual DbSet<ProductType> ProductTypes { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Product>()
                .ToTable("Products");
            modelBuilder.Entity<Product>(entity => {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Name).HasMaxLength(250);
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
            modelBuilder.Entity<ProductType>().HasData(
                new ProductType { Id = Guid.NewGuid(), Code = "AL", Name = "Αλεύρι" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΖΧ", Name = "Ζάχαρη" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΡΖ", Name = "Ρύζι" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΜΚ", Name = "Μακαρόνια" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΑΛ", Name = "Αλατι" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΚΚ", Name = "Κακάο" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΣΚ", Name = "Σοκολάτα" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΛΔ", Name = "Λάδι" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΞΔ", Name = "Ξύδι" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΚΒ", Name = "Κονσέρβες" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΦΡ", Name = "Φρούτα" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΞΚ", Name = "Ξυροί καρποί" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΣΛΔ", Name = "Σπορέλεα" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΚΦ", Name = "Καφές" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΤΣ", Name = "Τσάι" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΛΧ", Name = "Λαχανικά" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΚΡ", Name = "Κρέας" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΜΛ", Name = "Μέλι" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΦΡ", Name = "Φρυγανιές" },
                new ProductType { Id = Guid.NewGuid(), Code = "ΨΜ", Name = "Ψωμί" });
            //modelBuilder.Entity<Product>()
            //.HasData(
            //    new Product { Id = Guid.NewGuid(), Name = "Μακαρόνια", IsGlutenFree = true, Quantity = 1.5f, Weight = 2.5f, ExpirationDate = DateTime.Now.AddDays(35), CreationDate = DateTime.Now },
            //    new Product { Id = Guid.NewGuid(), Name = "Αλεύρι", IsGlutenFree = true, Quantity = 3.5f, Weight = 5.5f, ExpirationDate = DateTime.Now.AddDays(35), CreationDate = DateTime.Now }
            //) ;
        }

    }
}
