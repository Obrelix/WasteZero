namespace WasteZero.Models {
    public class ChartData {
        public string Name { get; set; } = string.Empty;
        public List<CosnumptionModel> Data { get; set; } = new List<CosnumptionModel>();
    }
}
