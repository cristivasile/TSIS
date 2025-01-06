using Newtonsoft.Json;
using System.Windows;
using Wpf_Interface;
using Wpf_Interface.Constants;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {  
            InitializeComponent();
        }

        private void TestConfig_Click(object sender, RoutedEventArgs e)
        {
            var testConfigWindow = new TestConfigWindow();
            testConfigWindow.Show();
            this.Close();
        }

        private void CompareConfigs_Click(object sender, RoutedEventArgs e)
        {
            var compareConfigsWindow = new CompareConfigsWindow();
            compareConfigsWindow.Show();
            this.Close();
        }
    }
}