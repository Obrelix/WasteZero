using System.Globalization;
using WasteZero.Models;

namespace WasteZero.Pages {
    public partial class Dashboard {
        bool showDataLabels = false;

        List<ChartData> consItems = new List<ChartData>();

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
            consItems = service.GetAllObjects();
        }
    }
}
