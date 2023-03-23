
using WasteZero.Data;
using Microsoft.EntityFrameworkCore;
using Radzen.Blazor;
using System;
using WasteZero.Models;
using Radzen;

namespace WasteZero.Pages
{
    public partial class Products {
        bool? groupsExpanded;
        Product? objToInsert;
        Product? objToUpdate;
        RadzenDataGrid<Product> grid;
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
            service.UpdateObj(product);
        }

        async Task SaveRow(Product product) {
            await grid.UpdateRow(product);
        }

        void CancelEdit(Product product) {
            if (product == objToInsert) 
                objToInsert = null;
            objToUpdate = null;
            grid.CancelEditRow(product);
            service.CancelEdit(product);
        }

        async Task DeleteRow(Product product) {
            if (product == objToInsert) 
                objToInsert = null;
            if (product == objToUpdate) 
                objToUpdate = null;
            if (products != null && products.Contains(product)) {
                service.DeleteRow(product);
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
            service.Create(product);
            objToInsert = null;
        }

        void OnGroupRowRender(GroupRowRenderEventArgs args) {
            args.Expanded = groupsExpanded;
        }

        void OnRender(DataGridRenderEventArgs<Product> args) {
            if (args.FirstRender) {
                args.Grid.Groups.Add(new GroupDescriptor() { Title = "Expiration Date", Property = "ExpirationDate", SortOrder = SortOrder.Ascending });
                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            products =  service.GetAllObjectsQuerable();
            productTypes = ptService.GetAllObjectsQuerable();
        }
    }
}
