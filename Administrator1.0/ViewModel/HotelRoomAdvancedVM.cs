using Administrator1._0.DBEntities;
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
    public class HotelRoomAdvancedVM : INotifyPropertyChanged
    {
        Db db;

        private Client selectedClient;
        public Client SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                SignalChanged();
            }
        }
        private RoomStatus selectedRoomStatus;
        public RoomStatus SelectedRoomStatus
        {
            get => selectedRoomStatus;
            set
            {
                selectedRoomStatus = value;
                SignalChanged();
            }
        }
        private RoomType selectedRoomType;
        public RoomType SelectedRoomType
        {
            get => selectedRoomType;
            set
            {
                selectedRoomType = value;
                SignalChanged();
            }
        }
        private HotelRoom selectedHotelRoom;
        public HotelRoom SelectedHotelRoom
        {
            get => selectedHotelRoom;
            set
            {
                selectedHotelRoom = value;
                SignalChanged();
            }
        }

        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<HotelRoom> HotelRooms { get; set; }
        public ObservableCollection<RoomStatus> RoomStatuses { get; set; }

        public CustomCommand HotelRoomSave { get; set; }


        public HotelRoomAdvancedVM()
        {
            db = Db.GetDb();
            Clients = new ObservableCollection<Client>(db.Clients);
            SignalChanged("Clients");
            RoomTypes = new ObservableCollection<RoomType>(db.RoomTypes);
            SignalChanged("RoomTypes");
            HotelRooms = new ObservableCollection<HotelRoom>(db.HotelRooms);
            SignalChanged("HotelRooms");
            RoomStatuses = new ObservableCollection<RoomStatus>(db.RoomStatuses);
            SignalChanged("RoomStatuses");
            HotelRoomSave = new CustomCommand(o =>
            {
                try
                {
                    db.SaveChanges();
                    HotelRooms = new ObservableCollection<HotelRoom>(db.HotelRooms);
                    SignalChanged("HotelRooms");

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }

            });


        }


        public HotelRoomAdvancedVM(HotelRoom selectedHotelRoom) : this()
        {
            SelectedHotelRoom = selectedHotelRoom;
        }
        protected void SignalChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
