using Administrator1._0.DBEntities;
using Administrator1._0.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Administrator1._0.ViewModel
{
    class MainVM : INotifyPropertyChanged
    {
        Db db;
        public ObservableCollection<Client> Clients { get; set; }

        public TourView TourView { get; set; }
        public ClientView ClientView { get; set; }
        public EtcView EtcView { get; set; }
        public HotelRoomView HotelRoomView { get; set; }
        public ServiceView ServiceView  { get; set;}
        public ConstructorView ConstructorView { get; set; }

        public CustomCommand ConstructorViewCommand { get; set; }
        public CustomCommand TourViewCommand { get; set; }
        public CustomCommand ClientViewCommand { get; set; }
        public CustomCommand CloseApp { get; set; }
        public CustomCommand WindowApp { get; set; }
        public CustomCommand ReduceApp { get; set; }
        public CustomCommand EtcViewCommand { get; set; }
        public CustomCommand HotelRoomViewCommand { get; set; }
        public CustomCommand ServiceViewCommand { get; set; }
        public CustomCommand Refresh { get; set; }

        private object _currentPage;
        public object CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                SignalChanged("CurrentPage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void SignalChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MainVM()
        {
            db = Db.GetDb();
            Clients = new ObservableCollection<Client>(db.Clients);
            int refreshid = 2;
            CurrentPage = new TourView();

            Refresh = new CustomCommand(o =>
            {
                if (refreshid == 2)
                    CurrentPage = new TourView();
                if (refreshid == 1)
                    CurrentPage = new HotelRoomView();
                if (refreshid == 3)
                    CurrentPage = new ClientView();
                if (refreshid == 4)
                    CurrentPage = new EtcView();
                if (refreshid == 5)
                    CurrentPage = new ServiceView();
                if (refreshid == 6)
                    CurrentPage = new ConstructorView();
                SignalChanged("CurrentPage");
            });
            HotelRoomViewCommand = new CustomCommand(o =>
            {
                refreshid = 1;
                CurrentPage = new HotelRoomView();
                SignalChanged("CurrentPage");
            });
            TourViewCommand = new CustomCommand(o =>
            {
                refreshid = 2;
                CurrentPage = new TourView();
                SignalChanged("CurrentPage");
            });
            ClientViewCommand = new CustomCommand(o =>
            {
                refreshid = 3;
                CurrentPage = new ClientView();
                SignalChanged("CurrentPage");
            });
            EtcViewCommand = new CustomCommand(o =>
            {
                refreshid = 4;
                CurrentPage = new EtcView();
                SignalChanged("CurrentPage");
            }); 
            ServiceViewCommand = new CustomCommand(o =>
            {
                refreshid = 5;
                CurrentPage = new ServiceView();
                SignalChanged("CurrentPage");
            });
            ConstructorViewCommand = new CustomCommand(o =>
            {
                refreshid = 6;
                CurrentPage = new ConstructorView();
                SignalChanged("CurrentPage");
            });
            CloseApp = new CustomCommand(o =>
            {
               System.Windows.Application.Current.Shutdown();
            });
            ReduceApp = new CustomCommand(o =>
            {
                System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
            });
        }
    }
}
