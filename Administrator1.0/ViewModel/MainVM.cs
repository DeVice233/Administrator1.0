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

        public CustomCommand TourViewCommand { get; set; }
        public CustomCommand ClientViewCommand { get; set; }
        public CustomCommand CloseApp { get; set; }
        public CustomCommand ReduceApp { get; set; }
        public CustomCommand EtcViewCommand { get; set; }
        public CustomCommand HotelRoomViewCommand { get; set; }
        public CustomCommand ServiceViewCommand { get; set; }

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
           
            CurrentPage = new TourView();

            HotelRoomViewCommand = new CustomCommand(o =>
            {
                CurrentPage = new HotelRoomView();
                SignalChanged("CurrentPage");
            });
            TourViewCommand = new CustomCommand(o =>
            {
                CurrentPage = new TourView();
                SignalChanged("CurrentPage");
            });
            ClientViewCommand = new CustomCommand(o =>
            {
                CurrentPage = new ClientView();
                SignalChanged("CurrentPage");
            });
            EtcViewCommand = new CustomCommand(o =>
            {
                CurrentPage = new EtcView();
                SignalChanged("CurrentPage");
            }); 
            ServiceViewCommand = new CustomCommand(o =>
            {
                CurrentPage = new ServiceView();
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
