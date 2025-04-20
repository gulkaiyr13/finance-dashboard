using FinanceDashboard.Models;
using FinanceDashboard.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceDashboard.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ISeries[] AAPLSeries { get; set; }
        public ISeries[] MSFTSeries { get; set; }
        public ISeries[] TSLASeries { get; set; }
        public ISeries[] UsdKgsSeries { get; set; }
        public ISeries[] BtcSeries { get; set; }

        public Axis[] SharedXAxes { get; set; }
        public Axis[] CurrencyAxes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task LoadAsync()
        {
            var service = new AlphaVantageService();
            var exchangeService = new ExchangeRateService();

            var aapl = await service.GetStockPricesAsync("AAPL");
            var msft = await service.GetStockPricesAsync("MSFT");
            var tsla = await service.GetStockPricesAsync("TSLA");

            var usdKgs = await exchangeService.GetUsdKgsAsync();
            var btc = await exchangeService.GetBtcUsdAsync();

            SharedXAxes = new Axis[]
            {
                new Axis { Labels = aapl.Select(d => d.Date.ToShortDateString()).ToArray(), LabelsRotation = 15 }
            };

            AAPLSeries = new ISeries[]
            {
                new LineSeries<double> { Name = "AAPL", Values = aapl.Select(d => (double)d.Price).ToArray(), GeometrySize = 5 }
            };

            MSFTSeries = new ISeries[]
            {
                new LineSeries<double> { Name = "MSFT", Values = msft.Select(d => (double)d.Price).ToArray(), GeometrySize = 5 }
            };

            TSLASeries = new ISeries[]
            {
                new LineSeries<double> { Name = "TSLA", Values = tsla.Select(d => (double)d.Price).ToArray(), GeometrySize = 5 }
            };

            UsdKgsSeries = new ISeries[]
            {
                new LineSeries<double> { Name = "USD/KGS", Values = usdKgs.Select(p => p.Rate).ToArray(), GeometrySize = 5 }
            };

            BtcSeries = new ISeries[]
            {
                new LineSeries<double> { Name = "BTC/USD", Values = btc.Select(p => p.Rate).ToArray(), GeometrySize = 5 }
            };

            CurrencyAxes = new Axis[]
            {
                new Axis { Labels = usdKgs.Select(p => p.Date.ToString("dd.MM")).ToArray(), LabelsRotation = 15 }
            };

            NotifyAll();
        }

        private void NotifyAll()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AAPLSeries)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MSFTSeries)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TSLASeries)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsdKgsSeries)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BtcSeries)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SharedXAxes)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyAxes)));
        }
    }
}
