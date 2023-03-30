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
        RadzenDataGrid<ProductType>? grid;
        IEnumerable<ProductType>? productTypes { get; set; }

        void Reset() {
            objToInsert = null;
            objToUpdate = null;
        }

        async Task EditRow(ProductType obj) {
            objToUpdate = obj;
            await grid!.EditRow(obj);
        }
        void OnUpdateRow(ProductType obj) {
            if (obj == objToInsert) {
                objToInsert = null;
            }
            objToUpdate = null;
            service.UpdateObj(obj);
        }

        async Task SaveRow(ProductType obj) {
            await grid!.UpdateRow(obj);
        }

        void CancelEdit(ProductType obj) {
            if (obj == objToInsert) 
                objToInsert = null;
            objToUpdate = null;
            grid!.CancelEditRow(obj);
            service.CancelEdit(obj);
        }

        async Task DeleteRow(ProductType obj) {
            if (obj == objToInsert) 
                objToInsert = null;
            if (obj == objToInsert) 
                objToInsert = null;

            if (productTypes != null && productTypes.Contains(obj)) {
            if (grid == null) return;
                service.DeleteRow(obj);
                await grid.Reload();
            } else {
                grid!.CancelEditRow(obj);
                await grid.Reload();
            }
        }

        async Task InsertRow() {
            objToInsert = new ProductType();
            objToInsert.Id = Guid.NewGuid();
            await grid!.InsertRow(objToInsert);
        }

        void OnCreateRow(ProductType obj) {
            service.Create(obj);
            objToInsert = null;
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            productTypes = service.GetAllObjectsQuerable();
        }

    }
}
