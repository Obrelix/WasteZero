using Microsoft.EntityFrameworkCore;
using WasteZero.Data;
using WasteZero.Models;
namespace WasteZero.Services {
    public class ProductTypeService {
        private readonly WasteZeroDbContext? dbContext;

        public ProductTypeService(WasteZeroDbContext context) {
            dbContext = context;
        }
        public IQueryable<ProductType>? GetAllObjectsQuerable() {
            IQueryable<ProductType>? result = dbContext?.ProductTypes?.AsQueryable();
            return result;
        }
        public void CancelEdit(ProductType obj) {
            var entry = dbContext?.Entry(obj);
            if (entry?.State == EntityState.Modified) {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }
        }
        public void DeleteRow(ProductType obj) {
            if (obj != null) {
                dbContext?.Remove<ProductType>(obj);
                dbContext?.SaveChanges();
            }
        }
        public void UpdateObj(ProductType obj) {
            if (obj != null) {
                dbContext?.Update<ProductType>(obj);
                dbContext?.SaveChanges();
            }
        }
        public void Create(ProductType obj) {
            if (obj != null) {
                dbContext?.Add(obj);
                dbContext?.SaveChanges();
            }
        }
    }
}
