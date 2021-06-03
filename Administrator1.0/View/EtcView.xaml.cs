﻿using Administrator1._0.ViewModel;
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
    /// Логика взаимодействия для EtcView.xaml
    /// </summary>
    public partial class EtcView : Page
    {
        public EtcView()
        {
            InitializeComponent();
            DataContext = new EtcVM();
        }
    }
}
