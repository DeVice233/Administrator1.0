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

namespace Administrator1._0.ViewModel
{
    public class MigrationVM : INotifyPropertyChanged
    {
        Db db;
        public class HotelRoomDate
        {
            public DateTime Date { get; set; }
            public int RoomNumber { get; set; }
            public string RoomTypeName { get; set; }
        }

        private Tour selectedTour = new Tour { Clients = new List<Client>() };
        public Tour SelectedTour
        {
            get => selectedTour;
            set
            {
                Set(ref selectedTour, value);
                SignalChanged();
            }
        }
        private HotelRoomDate selectedHotelRoomDate;
        public HotelRoomDate SelectedHotelRoomDate
        {
            get => selectedHotelRoomDate;
            set
            {
                Set(ref selectedHotelRoomDate, value);
                SignalChanged();
            }
        }
        private RoomType selectedRoomType;
        public RoomType SelectedRoomType
        {
            get => selectedRoomType;
            set
            {
                Set(ref selectedRoomType, value);
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

        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<HotelRoom> HotelRooms { get; set; }
        public ObservableCollection<RoomStatus> RoomStatuses { get; set; }
        public List<HotelRoomDate> HotelRoomDates { get; set; }

        public CustomCommand ChangeHotelRoom { get; set; }

        public MigrationVM()
        {
            db = Db.GetDb();
            Tours = new ObservableCollection<Tour>(db.Tours);
            SignalChanged("Tours");
            RoomTypes = new ObservableCollection<RoomType>(db.RoomTypes);
            SignalChanged("RoomTypes");
            RoomStatuses = new ObservableCollection<RoomStatus>(db.RoomStatuses);
            SignalChanged("RoomStatuses");
            HotelRooms = new ObservableCollection<HotelRoom>(db.HotelRooms);
            SignalChanged("HotelRooms");
            Clients = new ObservableCollection<Client>(db.Clients);
            SignalChanged("Clients");
            LoadHotelRoomDates();
            HotelRoomDates = new List<HotelRoomDate>(HotelRoomDates);

            ChangeHotelRoom = new CustomCommand(o =>
            {
                Change();
            });

        }

        public void Change()
        {
            HotelRoomDates = new List<HotelRoomDate>();
            int totaldays;
            int daystochangecount;
            totaldays = (int)(SelectedTour.LeaveDate - SelectedTour.ArriveDate).TotalDays;
            daystochangecount = (int)(SelectedTour.LeaveDate - SelectedHotelRoomDate.Date).TotalDays;

        }
        public void LoadHotelRoomDates()
        {
            HotelRoomDates = new List<HotelRoomDate>();
            DateTime arrivedate = SelectedTour.ArriveDate;
            int totaldays;
            totaldays = (int)(SelectedTour.LeaveDate - SelectedTour.ArriveDate).TotalDays;
            for (int i = 0; i <= totaldays; i++)
            {
                HotelRoomDate hotelRoomDate = new HotelRoomDate {
                Date = arrivedate,
                RoomNumber = SelectedTour.HotelRoom.RoomNumber,
                RoomTypeName = SelectedTour.HotelRoom.RoomType.RoomTypeName};
                HotelRoomDates.Add(hotelRoomDate);
                arrivedate = arrivedate.AddDays(1);
            }

        }

        public MigrationVM(Tour selectedTour) : this()
        {
            SelectedTour = selectedTour;
           
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
