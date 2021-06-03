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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Administrator1._0.View
{
    /// <summary>
    /// Логика взаимодействия для TourView.xaml
    /// </summary>
    public partial class TourView : Page
    {
        public TourView()
        {
            InitializeComponent();
            DataContext = new TourVM();
        }

        private void btnstatus_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = true;
            popup.Closed += (senderClosed, eClosed) =>
            {
                btnstatus.IsChecked = false;
            };
        }
        private void Popup_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Primitives.Popup popup = sender as System.Windows.Controls.Primitives.Popup;
            if (popup != null)
                popup.IsOpen = false;
        }

       
    }
}
