using FinanceDashboard.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinanceDashboard.Services
{
    public class ExchangeRateService
    {
        public async Task<List<ExchangeRatePoint>> GetUsdKgsAsync()
        {
            var url = $"https://api.exchangerate.host/timeseries?start_date=2024-12-01&end_date={DateTime.Now:yyyy-MM-dd}&base=USD&symbols=KGS";
            using var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var data = JObject.Parse(response);
            var rates = new List<ExchangeRatePoint>();

            var values = data["rates"] as JObject;
            foreach (var item in values)
            {
                rates.Add(new ExchangeRatePoint
                {
                    Date = DateTime.Parse(item.Key),
                    Rate = double.Parse(item.Value["KGS"].ToString(), CultureInfo.InvariantCulture)
                });
            }

            return rates.OrderBy(r => r.Date).ToList();
        }

        public async Task<List<ExchangeRatePoint>> GetBtcUsdAsync()
        {
            var url = "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=usd&days=90";
            using var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var data = JObject.Parse(response);
            var rates = new List<ExchangeRatePoint>();

            var prices = data["prices"];
            foreach (var item in prices)
            {
                var timestamp = (long)item[0];
                var date = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
                var price = (double)item[1];

                rates.Add(new ExchangeRatePoint
                {
                    Date = date,
                    Rate = price
                });
            }

            return rates.OrderBy(r => r.Date).ToList();
        }
    }
}
