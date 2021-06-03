using Administrator1._0.DBEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Administrator1._0.ViewModel
{
    class TourAdvancedVM : INotifyPropertyChanged
    {
        public ComboBoxItem SelectedType { get; set; }
        Db db;
        private readonly CollectionViewSource _FilterClients = new CollectionViewSource();

        public ICollectionView FilterClients => _FilterClients?.View;

        private void OnClientFiltred(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Client client))
            {
                e.Accepted = false;
                return;
            }

            var filter_text = _ClientFilterText;
            if (string.IsNullOrWhiteSpace(filter_text))
                return;
            if (client.FirstName is null || client.SecondName is null ||
                client.FatherName is null)
            {
                e.Accepted = false;
                return;
            }

            if ((string)SelectedType.Tag == "0")
                if (client.SecondName.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "1")
                if (client.FirstName.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "2")
                if (client.FatherName.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "3")
                if (client.Birthday.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "4")
                if (client.PassportNumber.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
           

            e.Accepted = false;
        }

        private string _ClientFilterText;
        public string ClientFilterText
        {
            get => _ClientFilterText;
            set
            {
                if (SelectedType == null)
                {
                    MessageBox.Show("Выберите тип поиска!");
                    return;
                }
                if (!Set(ref _ClientFilterText, value)) return;
                _FilterClients.View.Refresh();
            }
        }

        private bool _OnlyFree;
        public bool OnlyFree
        {
            get { return _OnlyFree; }
            set
            {
                if (!Set(ref _OnlyFree, value)) return;
                _FilterHotelRooms.View.Refresh();
                SignalChanged();
            }
        }

        private readonly CollectionViewSource _FilterHotelRooms = new CollectionViewSource();

        public ICollectionView FilterHotelRooms => _FilterHotelRooms?.View;
        private void OnHotelRoomFiltred(object sender, FilterEventArgs e)
        {
            if (!(e.Item is HotelRoom hotelRoom))
            {
                e.Accepted = false;
                return;
            }

            var onlyfree = _OnlyFree;
            if (onlyfree is false)
                return;

            if (hotelRoom.RoomNumber.ToString() is null || hotelRoom.RoomType is null ||
                hotelRoom.RoomStatus is null)
            {
                e.Accepted = false;
                return;
            }
            if (onlyfree is true)
                if (hotelRoom.RoomStatus.StatusName.Contains("Свободен", StringComparison.OrdinalIgnoreCase)) return;
            e.Accepted = false;
        }
        private Tour selectedTour = new Tour { Clients = new List<Client>()};
        public Tour SelectedTour
        {
            get => selectedTour;
            set
            {
                Set(ref selectedTour, value);
                SignalChanged();
            }
        }
        private Client selectedClient;
        public Client SelectedClient
        {
            get => selectedClient;
            set
            {
                Set(ref selectedClient, value);
                SignalChanged();
            }
        }
        private Client selectedFindClient;
        public Client SelectedFindClient
        {
            get => selectedFindClient;
            set
            {
                Set(ref selectedFindClient, value);
                SignalChanged();
            }
        }
        private HotelRoom selectedHotelRoom;
        public HotelRoom SelectedHotelRoom
        {
            get => selectedHotelRoom;
            set
            {
                Set(ref selectedHotelRoom, value);
                SignalChanged();
            }
        }

        public CustomCommand TourSave { get; set; }
        public CustomCommand AddSelectedRoom { get; set; }
        public CustomCommand AddClient { get; set; }
        public CustomCommand RemoveClient { get; set; }

        private string fullDays;
        public string FullDays
        {
            get => fullDays;
            set
            {
                Set(ref fullDays, value);
                SignalChanged();
            }
        }
        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<HotelRoom> HotelRooms { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<RoomStatus> RoomStatuses { get; set; }

        public ObservableCollection<Client> SelectedTourClients
        {
            get => new ObservableCollection<Client>(SelectedTour.Clients);

        }

        public TourAdvancedVM()
        {
            db = Db.GetDb();
            Clients = new ObservableCollection<Client>(db.Clients);
            SignalChanged("Clients");
            RoomTypes = new ObservableCollection<RoomType>(db.RoomTypes);
            SignalChanged("RoomTypes");
            HotelRooms = new ObservableCollection<HotelRoom>(db.HotelRooms);
            SignalChanged("HotelRooms");
            Tours = new ObservableCollection<Tour>(db.Tours);
            SignalChanged("Tours");
            RoomStatuses = new ObservableCollection<RoomStatus>(db.RoomStatuses);
            SignalChanged("RoomStatus");
            _FilterHotelRooms.Source = HotelRooms;
            _FilterHotelRooms.Filter += OnHotelRoomFiltred;
            _FilterClients.Source = Clients;
            _FilterClients.Filter += OnClientFiltred;
            FullDays = (SelectedTour.ArriveDate - SelectedTour.LeaveDate).TotalDays.ToString();
          
            TourSave = new CustomCommand(o =>
            {
                db.SaveChanges();
                Tours = new ObservableCollection<Tour>(db.Tours);
                SignalChanged("Tours");
                SignalChanged();
            });
            AddSelectedRoom = new CustomCommand(o =>
            {
                SelectedTour.HotelRoom = SelectedHotelRoom;
                SelectedHotelRoom.RoomStatus.StatusName = "Занят";
                db.SaveChanges();
                Tours = new ObservableCollection<Tour>(db.Tours);
                SignalChanged("Tours");
            });
            AddClient = new CustomCommand(o =>
            {
                if (SelectedFindClient != null)
                {
                    SelectedTour.Clients.Add(SelectedFindClient);
                    SignalChanged("SelectedTourClients");
                }
                else
                MessageBox.Show("Выберите клиента!");
            });
            RemoveClient = new CustomCommand(o =>
            {
                if (SelectedClient != null)
                {
                    SelectedTour.Clients.Remove(SelectedClient);
                    SignalChanged("SelectedTourClients");
                }
                else
                    MessageBox.Show("Выберите клиента!");
            });

        }


        public TourAdvancedVM(Tour selectedTour) : this()
        {
            SelectedTour = selectedTour;
            SelectedTour.Clients = db.Tours.Where(s => s == selectedTour).SelectMany(b => b.Clients).ToList();
        }
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        protected void SignalChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
