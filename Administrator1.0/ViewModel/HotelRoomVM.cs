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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Administrator1._0.ViewModel
{
    public class HotelRoomVM : INotifyPropertyChanged
    {
        public ComboBoxItem SelectedType { get; set; }
        Db db;
        public Window window { get; set; }
        public HotelRoomAdvanced hotelRoomAdvanced = new HotelRoomAdvanced();

        private HotelRoom selectedHotelRoom = new HotelRoom { };
        public HotelRoom SelectedHotelRoom
        {
            get => selectedHotelRoom;
            set
            {
                Set(ref selectedHotelRoom, value);
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

            var filter_text = _HotelRoomFilterText;
            if (string.IsNullOrWhiteSpace(filter_text))
                return;
            if (hotelRoom.RoomNumber.ToString() is null || hotelRoom.RoomType is null ||
                hotelRoom.RoomStatus is null)
            {
                e.Accepted = false;
                return;
            }
            if ((string)SelectedType.Tag == "0")
                if (hotelRoom.RoomNumber.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "1")
                if (hotelRoom.BedNumber.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "2")
                if (hotelRoom.RoomStatus.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "3")
                if (hotelRoom.RoomType.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;

            e.Accepted = false;
        }

        private string _HotelRoomFilterText;
        public string HotelRoomFilterText
        {
            get => _HotelRoomFilterText;
            set
            {
                if (SelectedType == null)
                {
                    MessageBox.Show("Выберите тип поиска!");
                    return;
                }
                if (!Set(ref _HotelRoomFilterText, value)) return;
                _FilterHotelRooms.View.Refresh();
                //Search();
            }
        }

        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<HotelRoom> HotelRooms { get; set; }
        public ObservableCollection<RoomStatus> RoomStatuses { get; set; }

        public CustomCommand OpenHotelRoomAdvanced {get; set; }
        public CustomCommand OpenNewHotelRoomAdvanced { get; set; }
        public CustomCommand DeleteHotelRoom { get; set; }

        public HotelRoomVM()
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
            _FilterHotelRooms.Source = HotelRooms;
            _FilterHotelRooms.Filter += OnHotelRoomFiltred;

            OpenHotelRoomAdvanced = new CustomCommand(o =>
            {
                window = new HotelRoomAdvanced(SelectedHotelRoom);
                window.ShowDialog();
            });
            DeleteHotelRoom = new CustomCommand(o =>
            {
                if (SelectedHotelRoom != null)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Удалить выбранный номер?", "Предупреждение",
                         MessageBoxButton.OKCancel, MessageBoxImage.Warning
                        );
                    if (messageBoxResult == MessageBoxResult.OK)
                    {
                        try
                        {
                            db.HotelRooms.Remove(SelectedHotelRoom);
                            db.SaveChanges();
                            HotelRooms = new ObservableCollection<HotelRoom>(db.HotelRooms);
                            SignalChanged("HotelRooms");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            });
            OpenNewHotelRoomAdvanced = new CustomCommand(o =>
            {
                //MessageBoxResult Result = MessageBox.Show(
                //    "Создать новый номер?",
                //    "Сообщение",
                //    MessageBoxButton.OKCancel,
                //    MessageBoxImage.Question
                //    );
                //if (Result == MessageBoxResult.Yes)
                //{
                    var hotelRoom = new HotelRoom { };
                    db.HotelRooms.Add(hotelRoom);
                    SelectedHotelRoom = hotelRoom;
                    HotelRooms = new ObservableCollection<HotelRoom>(db.HotelRooms);
                    SignalChanged("HotelRooms");
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
                    window = new HotelRoomAdvanced(SelectedHotelRoom);
                    window.ShowDialog();
                //}
            });
        }

       
        //private void Search()
        //{
        //    if ((string)SelectedType.Tag == "0")
        //        SelectedHotelRoom = HotelRooms.FirstOrDefault(s => s.RoomNumber.ToString().StartsWith(Pattern));
        //    else if ((string)SelectedType.Tag == "1")
        //        SelectedHotelRoom = HotelRooms.FirstOrDefault(s => s.RoomStatus.StatusName.ToString().StartsWith(Pattern));
        //    else if ((string)SelectedType.Tag == "2")
        //        SelectedHotelRoom = HotelRooms.FirstOrDefault(s => s.RoomType.RoomTypeName.ToString().StartsWith(Pattern));
        //}
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
