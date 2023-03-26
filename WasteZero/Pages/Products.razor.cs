
using WasteZero.Data;
using Microsoft.EntityFrameworkCore;
using Radzen.Blazor;
using System;
using WasteZero.Models;
using Radzen;

namespace WasteZero.Pages
{
    public partial class Products {
        Product? objToInsert;
        Product? objToUpdate;
        ProductDetail? objToInsertDetail;
        ProductDetail? objToUpdateDetail;
        RadzenDataGrid<Product>? grid;
        RadzenDataGrid<ProductDetail>? gridDetail;
        IEnumerable<Product>? products { get; set; }
        IEnumerable<ProductDetail>? details { get; set; }
        IEnumerable<ProductType>? productTypes { get; set; }

        void Reset() {
            objToInsert = null;
            objToUpdate = null;
        }

        async Task EditRow(Product product) {
            if (grid == null) return;
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
            if (grid == null) return;
            await grid.UpdateRow(product);
        }

        void CancelEdit(Product product) {
            if (grid == null) return;
            if (product == objToInsert) 
                objToInsert = null;
            objToUpdate = null;
            grid.CancelEditRow(product);
            service.CancelEdit(product);
        }

        async Task DeleteRow(Product product) {
            if (grid == null) return;
            //var confirmationResult = await this.DialogService.Confirm("Are you sure?", "Delete Row", new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
            //if (confirmationResult == true) {
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
            //}
        }

        async Task InsertRow() {
            if (grid == null) return;
            objToInsert = new Product();
            objToInsert.Id = Guid.NewGuid();
            await grid.InsertRow(objToInsert);
        }

        void OnCreateRow(Product product) {
            service.Create(product);
            products = service.GetAllObjectsQuerable();
            objToInsert = null;
        }

        void OnGroupRowRender(GroupRowRenderEventArgs args) {
            args.Expanded = true;
        }

        void OnRender(DataGridRenderEventArgs<Product> args) {
            if (args.FirstRender) {
                //args.Grid.Groups.Add(new GroupDescriptor() { Title = "Expiration Date", Property = "ExpirationDate", SortOrder = SortOrder.Ascending });
                //StateHasChanged();
            }
        }

        void RowRender(RowRenderEventArgs<Product> args) {
            args.Expandable = true;
            args.Attributes.Add("style", $"font-weight: {(args.Data.Status != ExprirationStatus.ok ? "bold" : "normal")};");
        }

        void CellRender(DataGridCellRenderEventArgs<Product> args) {
            if (args.Column.Property != "Name") 
                return; 
            ExprirationStatus? status = args.Data.Status;
            switch (status) {
                case ExprirationStatus.noDetails:
                    args.Attributes.Add("style", $"background-color: var(--rz-base-500)");
                    break;
                case ExprirationStatus.ok:
                    args.Attributes.Add("style", $"background-color: var(--rz-base-background-color)");
                    break;
                case ExprirationStatus.soon:
                    args.Attributes.Add("style", $"background-color: var(--rz-warning-lighter)");
                    break;
                case ExprirationStatus.almost:
                    args.Attributes.Add("style", $"background-color: var(--rz-warning)");
                    break;
                case ExprirationStatus.expired:
                    args.Attributes.Add("style", $"background-color: var(--rz-danger-light)");
                    break;
                default:
                    args.Attributes.Add("style", $"background-color: var(--rz-base-background-color)");
                    break;
            }
        }

        void RowRenderDetail(RowRenderEventArgs<ProductDetail> args) {
            args.Expandable = true;
            args.Attributes.Add("style", $"font-weight: {(args.Data.Status != ExprirationStatus.ok ? "bold" : "normal")};");
        }

        void CellRenderDetail(DataGridCellRenderEventArgs<ProductDetail> args) {
            if (args.Column.Property != "ExpirationDate")
                return;
            ExprirationStatus? status = args.Data.Status;
            switch (status) {
                case ExprirationStatus.noDetails:
                    args.Attributes.Add("style", $"background-color: var(--rz-base-500)");
                    break;
                case ExprirationStatus.ok:
                    args.Attributes.Add("style", $"background-color: var(--rz-base-background-color)");
                    break;
                case ExprirationStatus.soon:
                    args.Attributes.Add("style", $"background-color: var(--rz-warning-lighter)");
                    break;
                case ExprirationStatus.almost:
                    args.Attributes.Add("style", $"background-color: var(--rz-warning)");
                    break;
                case ExprirationStatus.expired:
                    args.Attributes.Add("style", $"background-color: var(--rz-danger-light)");
                    break;
                default:
                    args.Attributes.Add("style", $"background-color: var(--rz-base-background-color)");
                    break;
            }
        }

        void RowExpand(Product obj) {
            if (obj.Details == null) {
                obj.Details = service.GetAllDetailsQuerable(obj.Id);
            }
        }

        void ResetDetail() {
            objToInsertDetail = null;
            objToUpdateDetail = null;
        }

        async Task EditRowDetail(ProductDetail detail) {
            if (gridDetail == null) return;
            objToUpdateDetail = detail;
            await gridDetail.EditRow(objToUpdateDetail);
        }

        void OnUpdateRowDetail(ProductDetail detail) {
            if (detail == objToInsertDetail)
                objToInsertDetail = null;
            objToUpdateDetail = null;
            service.UpdateObjDetail(detail);
        }

        async Task SaveRowDetail(ProductDetail detail) {
            if (gridDetail == null) return;
            await gridDetail.UpdateRow(detail);
        }

        void CancelEditDetail(ProductDetail detail) {
            if (gridDetail == null) return;
            if (detail == objToInsertDetail)
                objToInsertDetail = null;
            objToUpdateDetail = null;
            gridDetail.CancelEditRow(detail);
            service.CancelEditDetail(detail);
        }

        async Task DeleteRowDetail(ProductDetail detail) {
            if (gridDetail == null) return;
            //var confirmationResult = await this.DialogService.Confirm("Are you sure?", "Delete Row", new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
       //     var confirmResult = await DialogService.Confirm(
       //"Foo", "Bar");
       //     if (confirmResult.HasValue && confirmResult.Value) {
                if (detail == objToInsertDetail)
                    objToInsertDetail = null;
                if (detail == objToUpdateDetail)
                    objToUpdateDetail = null;
                if (details != null && details.Contains(detail)) {
                    service.DeleteRowDetail(detail);
                    await gridDetail.Reload();
                } else {
                    gridDetail.CancelEditRow(detail);
                    await gridDetail.Reload();
                }
            //}
        }

        async Task InsertRowDetail(Guid parentID) {
            if (gridDetail == null) return;
            objToInsertDetail = new ProductDetail();
            objToInsertDetail.Id = Guid.NewGuid();
            objToInsertDetail.ProductID = parentID;
            objToInsertDetail.AddedDate = DateTime.Now;
            await gridDetail.InsertRow(objToInsertDetail);
        }

        void OnCreateRowDetail(ProductDetail detail) {
            service.CreateDetail(detail);
            objToInsertDetail = null;
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            products =  service.GetAllObjectsQuerable();
            productTypes = ptService.GetAllObjectsQuerable();
        }
    }
}
