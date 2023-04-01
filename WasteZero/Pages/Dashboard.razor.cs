using Radzen;
using Radzen.Blazor;
using System;
using System.Globalization;
using WasteZero.Models;

namespace WasteZero.Pages {
    public partial class Dashboard {
        List<Guid>? productIDs;
        List<ChartData>? consItems;
        List<ChartData>? selectedData;
        RadzenDataGrid<Product>? grid;
        RadzenDataGrid<ProductDetail>? gridDetail;
        List<Product>? expiredProducts { get; set; }
        List<Product>? products { get; set; }
        IEnumerable<ProductType>? productTypes { get; set; }
        DateTime startDate { get; set; } = DateTime.Now.AddYears(-1).Date;
        DateTime endDate { get; set; } = DateTime.Now.AddMonths(2).Date;
        string FormatAsNumeric(object value) {
            return ((double)value).ToString("N", CultureInfo.CreateSpecificCulture("en-US"));
        }

        string FormatAsMonth(object value) {
            if (value != null) {
                return Convert.ToDateTime(value).ToString("dd/MM/yy");
            }

            return string.Empty;
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            consItems = service.GetAllObjectsCons(startDate, endDate);
            expiredProducts = pservice.GetExpiredObjects();
            products = pservice.GetAllObjectsQuerable()?.ToList();
            productTypes = ptService.GetAllObjectsQuerable();
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
                obj.Details = pservice.GetAllDetailsQuerable(obj.Id);
            }
        }

        void ChangeChartData(object value) {
            if (selectedData == null)
                selectedData = new List<ChartData>();
            else 
                selectedData.Clear();
            consItems = service.GetAllObjectsCons(startDate, endDate);
            if (productIDs != null && consItems != null)
                selectedData.AddRange(consItems.Where(x => productIDs.Contains(x.ProductID)));
        }
        void ChangeChartData(DateTime? date) {
            if (selectedData == null)
                selectedData = new List<ChartData>();
            else
                selectedData.Clear();
            if (!date.HasValue) return;
            consItems = service.GetAllObjectsCons(startDate, endDate);
            if (productIDs != null && consItems != null )
                selectedData.AddRange(consItems.Where(x => productIDs.Contains(x.ProductID)));
        }
    }
}
