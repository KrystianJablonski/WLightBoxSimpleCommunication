using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VM;

namespace WindowApplicationProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (DataContext is ViewModel viewModel)
            {
                viewModel.ShowMessage = message => MessageBox.Show(message);
                viewModel.DeviceAddressEnteredCorrectly = DeviceAddressEnteredCorrectly;
            }

            ConnectGrid.Visibility = Visibility.Visible;
            ManagementGrid.Visibility = Visibility.Hidden;
        }

        private void DeviceAddressEnteredCorrectly()
        {
            ConnectGrid.Visibility = Visibility.Hidden;
            ManagementGrid.Visibility = Visibility.Visible;
        }
    }
}
