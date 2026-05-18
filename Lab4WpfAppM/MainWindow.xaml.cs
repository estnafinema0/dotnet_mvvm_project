using System.Windows;
using TestDataViewModel;

namespace Lab4WpfAppM
{
    public partial class MainWindow : Window, IUIServices
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this);
        }

        public void ShowMessage(string message, string caption) =>
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
    }
}