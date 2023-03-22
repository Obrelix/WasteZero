
using WasteZero.Data;
using Microsoft.EntityFrameworkCore;
using Radzen.Blazor;
using System;
using WasteZero.Models;

namespace WasteZero.Pages
{
    public partial class Products {
        Product? objToInsert;
        Product? objToUpdate;
        RadzenDataGrid<Product> grid;
        private WasteZeroDbContext? dbContext;
        IEnumerable<Product>? products { get; set; } 
        IEnumerable<ProductType>? productTypes { get; set; }

        void Reset() {
            objToInsert = null;
            objToUpdate = null;
        }

        async Task EditRow(Product product) {
            objToUpdate = product;
            await grid.EditRow(product);
        }
        void OnUpdateRow(Product product) {
            if (product == objToInsert) 
                objToInsert = null;
            objToUpdate = null;
            dbContext?.Update(product);
            dbContext?.SaveChanges();
        }

        async Task SaveRow(Product product) {
            await grid.UpdateRow(product);
        }

        void CancelEdit(Product product) {
            if (product == objToInsert) 
                objToInsert = null;
            objToUpdate = null;
            grid.CancelEditRow(product);
            var entry = dbContext?.Entry(product);
            if (entry?.State == EntityState.Modified) {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }
        }

        async Task DeleteRow(Product product) {
            if (product == objToInsert) 
                objToInsert = null;
            if (product == objToUpdate) 
                objToUpdate = null;
            if (products.Contains(product)) {
                dbContext?.Remove<Product>(product);
                dbContext?.SaveChanges();
                await grid.Reload();
            } else {
                grid.CancelEditRow(product);
                await grid.Reload();
            }
        }

        async Task InsertRow() {
            objToInsert = new Product();
            objToInsert.Id = Guid.NewGuid();
            await grid.InsertRow(objToInsert);
        }

        void OnCreateRow(Product product) {
            dbContext?.Add(product);
            dbContext?.SaveChanges();
            objToInsert = null;
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            dbContext ??= await ProjectDataContextFactory.CreateDbContextAsync();
            products =  dbContext.Products.Include("ProductType");
            productTypes = dbContext.ProductTypes;
        }
    }
}
