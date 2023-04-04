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
        public List<ChartData> GetAllObjectsCons(DateTime from, DateTime to) {
            List<Product>? data = dbContext?.Products?
                .Include(x => x.ProductType)
                .Include(x => x.Details)
                .Include(x => x.ConsumedDetails)
                .ToList();
            List<ChartData> result = new List<ChartData>();
            List<DateTime> dates = GetDates(from,to);
            if (data != null) 
                foreach (Product obj in data)  {
                    if(obj == null ) continue;
                    List<CosnumptionModel> lst = new List<CosnumptionModel>();
                    foreach (DateTime dt in dates) {
                        CosnumptionModel model = new CosnumptionModel();
                        model.Name = obj!.Name;
                        model.Date = dt;
                        float consumedWeight = 0.0f;
                        float addedWeight = 0.0f;
                        int consumedCount = 0;
                        int addedCount = 0;
                        if (obj?.ConsumedDetails != null ) {
                            List<ConsumedDetail> consumedDetails = obj.ConsumedDetails.Where(x => x.ParentAddedDate.HasValue && GetStartOfMonth(x.ParentAddedDate.Value) <= model.Date &&
                                                                                                  x.AddedDate.HasValue && GetStartOfMonth(x.AddedDate.Value) > model.Date).ToList();
                            if(consumedDetails != null && consumedDetails.Any()) {
                                consumedWeight = consumedDetails.Where(x=>x.Weight.HasValue).Sum(x => x.Weight!.Value);
                                consumedCount = consumedDetails.Count;
                            }
                        }
                        if (obj?.Details != null) {
                            List<ProductDetail> details = obj.Details.Where(x => x.AddedDate.HasValue && GetStartOfMonth(x.AddedDate.Value) <= model.Date).ToList();
                            if (details != null && details.Any()) {
                                addedWeight = details.Where(x => x.Weight.HasValue).Sum(x => x.Weight!.Value);
                                addedCount = details.Count;
                            }
                        }
                        model.Weight = addedWeight + consumedWeight;
                        model.StockCount = addedCount + consumedCount;
                        model.AddedCount = obj?.Details?.Count(x => x.AddedDate.HasValue && GetStartOfMonth(x.AddedDate.Value) == model.Date ) + obj?.ConsumedDetails?.Count(x => x.ParentAddedDate.HasValue && GetStartOfMonth(x.ParentAddedDate.Value) == model.Date);
                        model.ConsumedCount = obj?.ConsumedDetails?.Count(x => x.AddedDate.HasValue && GetStartOfMonth(x.AddedDate.Value) == model.Date);
                        lst.Add(model);
                    }
                    if(lst.Any()) {
                        ChartData chart = new ChartData();
                        chart.ProductID = obj.Id;
                        chart.Name = obj.Name!;
                        chart.Data = lst;
                        result.Add(chart);
                    }
                }
            var x = 0;
            x ++;
            return result;
        }
        private DateTime GetStartOfMonth(DateTime date) {
            return new DateTime(date.Year, date.Month, 1).Date;
        }
        private List<DateTime> GetDates(DateTime from, DateTime to) {
            List<DateTime> result = new List<DateTime>();
            int totalMonths = Convert.ToInt32(to.Subtract(from).Days / (365.25 / 12));
            if(totalMonths>0)
                for (int i = 0; i <= totalMonths; i++) {
                    DateTime date = GetStartOfMonth( from.AddMonths(i).Date);
                    result.Add(date);
                }
            return result;
        }
    }
}
