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
    /// Логика взаимодействия для Migration.xaml
    /// </summary>
    public partial class Migration : Window
    {
        public Migration()
        {
            InitializeComponent();
            DataContext = new MigrationVM();
            
        }
        public Migration(Tour selectedTour)
        {
            InitializeComponent();
            DataContext = new MigrationVM(selectedTour);
        }

    }
}
