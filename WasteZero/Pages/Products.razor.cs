
using WasteZero.Data;
using Microsoft.EntityFrameworkCore;
using Radzen.Blazor;
using System;
using WasteZero.Models;
using Radzen;

namespace WasteZero.Pages {
    public partial class Products {
        Product? objToInsert;
        Product? objToUpdate;
        ProductDetail? objToInsertDetail;
        ProductDetail? objToUpdateDetail;
        RadzenDataGrid<Product>? grid;
        RadzenDataGrid<ProductDetail>? gridDetail;
        IEnumerable<Product>? products { get; set; }
        IEnumerable<ProductType>? productTypes { get; set; }
        public bool EditEnabled { get; set; } = false;
        public bool EditDetailEnabled { get; set; } = false;
        public bool FromMasterInsert { get; set; } = false; 
        string colNameWidth = "25%";
        string colTypeWidth = "15%";
        string colGFWidth = "10%";
        string colQTYWidth = "15%";
        string colWGHWidth = "10%";
        string colActionsWidth = "30%";
        void Reset() {
            objToInsert = null;
            objToUpdate = null;
        }

        async Task EditRow(Product product) {
            objToUpdate = product;
            await grid!.EditRow(product);
        }

        void OnUpdateRow(Product product) {
            if (product == objToInsert)
                objToInsert = null;
            objToUpdate = null;
            service.UpdateObj(product);
        }

        async Task SaveRow(Product product) {
            await grid!.UpdateRow(product);
        }

        void CancelEdit(Product product) {
            if (product == objToInsert)
                objToInsert = null;
            objToUpdate = null;
            grid!.CancelEditRow(product);
            service.CancelEdit(product);
        }

        async Task DeleteRow(Product product) {
            var confirmationResult = await this.DialogService.Confirm("Are you sure ?", "Delete Row Permanently ", new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
            if (confirmationResult.HasValue && confirmationResult.Value) {
                if (product == objToInsert)
                    objToInsert = null;
                if (product == objToUpdate)
                    objToUpdate = null;
                if (products != null && products.Contains(product)) {
                    service.DeleteRow(product);
                    await grid!.Reload();
                } else {
                    grid!.CancelEditRow(product);
                    await grid!.Reload();
                }
            }
        }

        async Task InsertRow() {
            FromMasterInsert = true;
            objToInsert = new Product();
            objToInsert.Id = Guid.NewGuid();
            objToInsert.Details = new List<ProductDetail>();
            await grid!.InsertRow(objToInsert);
            await grid!.ExpandRow(objToInsert);
        }

        async Task OnCreateRow(Product product) {
            service.Create(product);
            products = service.GetAllObjectsQuerable();
            products = products?.OrderBy(x => x.MinDetailExpDate).ThenBy(x => x.Name).AsQueryable();
            await grid!.CollapseRows(new List<Product>() { objToInsert! });
            objToInsert = null;
            objToInsertDetail = null;
            FromMasterInsert = false;
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
                case ExprirationStatus.soon:
                    args.Attributes.Add("style", $"background-color: var(--rz-warning-lighter)");
                    break;
                case ExprirationStatus.almost:
                    args.Attributes.Add("style", $"background-color: var(--rz-warning)");
                    break;
                case ExprirationStatus.expired:
                    args.Attributes.Add("style", $"background-color: var(--rz-danger-light)");
                    break;
                case ExprirationStatus.ok:
                default:
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
                case ExprirationStatus.soon:
                    args.Attributes.Add("style", $"background-color: var(--rz-warning-lighter)");
                    break;
                case ExprirationStatus.almost:
                    args.Attributes.Add("style", $"background-color: var(--rz-warning)");
                    break;
                case ExprirationStatus.expired:
                    args.Attributes.Add("style", $"background-color: var(--rz-danger-light)");
                    break;
                case ExprirationStatus.ok:
                default:
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
            objToUpdateDetail = detail;
            await gridDetail!.EditRow(objToUpdateDetail);
        }

        void OnUpdateRowDetail(ProductDetail detail) {
            if (detail == objToInsertDetail)
                objToInsertDetail = null;
            objToUpdateDetail = null;
            service.UpdateObjDetail(detail);
        }

        async Task SaveRowDetail(ProductDetail detail) {
            await gridDetail!.UpdateRow(detail);
        }

        void CancelEditDetail(ProductDetail detail) {
            if (detail == objToInsertDetail)
                objToInsertDetail = null;
            objToUpdateDetail = null;
            gridDetail!.CancelEditRow(detail);
            service.CancelEditDetail(detail);
        }

        async Task DeleteRowDetail(ProductDetail detail, bool consume) {
            string title = consume ? "Consume Product" : "Delete Row";
            var confirmationResult = await this.DialogService.Confirm("Are you sure?", title, new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
            if (confirmationResult.HasValue && confirmationResult.Value) {
                if (detail == objToInsertDetail)
                    objToInsertDetail = null;
                if (detail == objToUpdateDetail)
                    objToUpdateDetail = null;
                Product? obj = products?.Where(x => x.Id.Equals(detail.ProductID)).FirstOrDefault();
                if (obj != null && obj.Details != null && obj.Details.Any(x => x.Id.Equals(detail.Id))) {
                    if (!consume)
                        service.DeleteRowDetail(detail);
                    else
                        service.ConsumeRowDetail(detail);
                    await gridDetail!.Reload();
                } else {
                    gridDetail!.CancelEditRow(detail);
                    await gridDetail.Reload();
                }
            }
        }

        async Task InsertRowDetail(Guid parentID) {
            objToInsertDetail = new ProductDetail();
            objToInsertDetail.Id = Guid.NewGuid();
            objToInsertDetail.ProductID = parentID;
            objToInsertDetail.AddedDate = DateTime.Now;
            await gridDetail!.InsertRow(objToInsertDetail);
            if(FromMasterInsert && objToInsert != null && objToInsert.Details!=null) {
                List<ProductDetail> details = (List<ProductDetail>)objToInsert.Details;
                details!.Add(objToInsertDetail);
            }
        }

        void ChangeEditStatus() {
            EditEnabled = !EditEnabled;
        }

        void ChangeEditDetailStatus() {
            EditDetailEnabled = !EditDetailEnabled;
        }

        void OnCreateRowDetail(ProductDetail detail) {
            service.CreateDetail(detail);
            objToInsertDetail = null;
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            products = service.GetAllObjectsQuerable();
            products = products?.OrderBy(x => x.MinDetailExpDate).ThenBy(x => x.Name).AsQueryable();
            productTypes = ptService.GetAllObjectsQuerable();
        }
    }
}
