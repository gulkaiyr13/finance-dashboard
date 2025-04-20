using FinanceDashboard.Models;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace FinanceDashboard.Services
{
    public class AlphaVantageService
    {
        private const string ApiKey = "OPIM1P0A78VN38FI";

        public async Task<List<StockPrice>> GetStockPricesAsync(string symbol)
        {
            var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={ApiKey}";
            using var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var data = JObject.Parse(response);
            var prices = new List<StockPrice>();

            var timeSeries = data["Time Series (Daily)"] as JObject;
            if (timeSeries == null) return prices;

            foreach (var item in timeSeries)
            {
                prices.Add(new StockPrice
                {
                    Date = DateTime.Parse(item.Key),
                    Price = decimal.Parse(item.Value["4. close"].ToString(), CultureInfo.InvariantCulture)
                });
            }

            return prices.OrderBy(p => p.Date).ToList();
        }
    }
}