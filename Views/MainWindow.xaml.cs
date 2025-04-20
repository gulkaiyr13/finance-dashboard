using System.Windows;
using FinanceDashboard.ViewModels;

namespace FinanceDashboard.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var vm = DataContext as MainViewModel;
            _ = vm?.LoadAsync();
        }
    }
}