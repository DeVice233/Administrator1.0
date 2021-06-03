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
    /// Логика взаимодействия для HotelRoomAdvanced.xaml
    /// </summary>
    public partial class HotelRoomAdvanced : Window
    {
        public HotelRoomAdvanced()
        {
            InitializeComponent();

        }
        public HotelRoomAdvanced(HotelRoom selectedHotelRoom)
        {
            InitializeComponent();
            DataContext = new HotelRoomAdvancedVM(selectedHotelRoom);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
