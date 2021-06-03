using Administrator1._0.DBEntities;
using Administrator1._0.ViewModel;
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
using System.Windows.Shapes;

namespace Administrator1._0.View
{
    /// <summary>
    /// Логика взаимодействия для ServiceAdvanced.xaml
    /// </summary>
    public partial class ServiceAdvanced : Window
    {
        public ServiceAdvanced()
        {
            InitializeComponent();
        }
        public ServiceAdvanced(Service selectedService)
        {
            InitializeComponent();
            DataContext = new ServiceAdvancedVM(selectedService);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
