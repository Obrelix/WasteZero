using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WasteZero.Data;
using WasteZero.Models;
namespace WasteZero.Services {
    public class ChartService {
        private readonly WasteZeroDbContext? dbContext;
        public ChartService(WasteZeroDbContext context) {
            dbContext = context;
        }
        public List<ChartData> GetAllObjects() {
            List<Product>? data = dbContext?.Products?
                .Include(x => x.ProductType)
                .Include(x => x.Details)
                .Include(x => x.ConsumedDetails)
                .ToList();
            List<ChartData> result = new List<ChartData>();
            List<DateTime> dates = GetDates();
            int counter = 0;
            if (data != null) 
                foreach (Product obj in data)  {
                    List<CosnumptionModel> lst = new List<CosnumptionModel>();
                    foreach (DateTime dt in dates) {
                        CosnumptionModel model = new CosnumptionModel();
                        model.Name = obj.Name;
                        model.Date = dt;
                        float consumedWeight = 0.0f;
                        float addedWeight = 0.0f;
                        int consumedCount = 0;
                        int addedCount = 0;
                        if (obj.ConsumedDetails != null) {
                            consumedWeight = obj.ConsumedDetails.Where(x => x.AddedDate <= model.Date).Sum(x => x.Weight!.Value);
                            consumedCount = obj.ConsumedDetails.Count(x => x.AddedDate <= model.Date);
                        }
                        if (obj.Details != null) {
                            addedWeight = obj.Details.Where(x => x.AddedDate <= model.Date).Sum(x => x.Weight!.Value);
                            addedCount = obj.Details.Count(x => x.AddedDate <= model.Date);
                        }
                        model.Weight = addedWeight - consumedWeight;
                        model.StockCount = addedCount - consumedCount; 
                        model.AddedCount = obj.Details?.Count(x => x.AddedDate >= model.Date && x.AddedDate <= model.Date.Value.AddMonths(1));
                        model.ConsumedCount = obj.ConsumedDetails?.Count(x => x.AddedDate >= model.Date && x.AddedDate <= model.Date.Value.AddMonths(1));
                        lst.Add(model);
                    }
                    if(lst.Any()) {
                        ChartData chart = new ChartData();
                        chart.Name = obj.Name;
                        chart.Data = lst;
                        result.Add(chart);
                    }
                    counter++;
                    if (counter >= 5) break;
                }
            return result;
        }
        private List<DateTime> GetDates() {
            List<DateTime> result = new List<DateTime>();
                for(int i = -12; i <= 2; i++) {
                    DateTime date = DateTime.Now.AddMonths(i).Date;
                    date = new DateTime(date.Year, date.Month, 1);
                    result.Add(date);
                }
            return result;
        }
    }
}
