
using WasteZero.Data;
using Microsoft.EntityFrameworkCore;
using Radzen.Blazor;
using System;
using WasteZero.Models;

namespace WasteZero.Pages
{
    public partial class ProductTypes {
        ProductType? objToInsert;
        ProductType? objToUpdate;
        RadzenDataGrid<ProductType> grid;
        private WasteZeroDbContext? _context;
        IEnumerable<ProductType>? productTypes { get; set; }

        void Reset() {
            objToInsert = null;
            objToUpdate = null;
        }

        async Task EditRow(ProductType obj) {
            objToUpdate = obj;
            await grid.EditRow(obj);
        }
        void OnUpdateRow(ProductType obj) {
            if (obj == objToInsert) {
                objToInsert = null;
            }
            objToUpdate = null;
            _context?.Update(obj);
            _context?.SaveChanges();
        }

        async Task SaveRow(ProductType obj) {
            await grid.UpdateRow(obj);
        }

        void CancelEdit(ProductType obj) {
            if (obj == objToInsert) 
                objToInsert = null;
            objToUpdate = null;
            grid.CancelEditRow(obj);
            var entry = _context?.Entry(obj);
            if (entry?.State == EntityState.Modified) {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }
        }

        async Task DeleteRow(ProductType obj) {
            if (obj == objToInsert) 
                objToInsert = null;
            if (obj == objToInsert) 
                objToInsert = null;

            if (productTypes.Contains(obj)) {
                _context?.Remove<ProductType>(obj);
                _context?.SaveChanges();
                productTypes = _context?.ProductTypes;
                await grid.Reload();
            } else {
                grid.CancelEditRow(obj);
                await grid.Reload();
            }
        }

        async Task InsertRow() {
            objToInsert = new ProductType();
            objToInsert.Id = Guid.NewGuid();
            await grid.InsertRow(objToInsert);
        }

        void OnCreateRow(ProductType obj) {
            _context?.Add(obj);
            _context?.SaveChanges();
            productTypes = _context?.ProductTypes;
            objToInsert = null;
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            _context ??= await ProjectDataContextFactory.CreateDbContextAsync();
            productTypes = _context.ProductTypes;
        }

    }
}
