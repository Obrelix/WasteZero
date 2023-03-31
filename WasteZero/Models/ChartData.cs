namespace WasteZero.Models {
    public class ChartData {
        public Guid ProductID { get; set; } 
        public string Name { get; set; } = string.Empty;
        public List<CosnumptionModel> Data { get; set; } = new List<CosnumptionModel>();
    }
}
