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
    class ConstructorVM : INotifyPropertyChanged
    {
        Db db;
        public ComboBoxItem SelectedAdultCount { get; set; }
        public ComboBoxItem SelectedChildCount { get; set; }

        private readonly CollectionViewSource _FilterHotelRooms = new CollectionViewSource();

        public ICollectionView FilterHotelRooms => _FilterHotelRooms?.View;

        private void OnHotelRoomFiltred(object sender, FilterEventArgs e)
        {
            if (!(e.Item is HotelRoom hotelRoom))
            {
                e.Accepted = false;
                return;
            }

            var _countBedText = countBedText;
            if (string.IsNullOrWhiteSpace(_countBedText))
                return;

            if (hotelRoom.RoomNumber.ToString() is null || hotelRoom.RoomType is null ||
                hotelRoom.RoomStatus is null || SelectedRoomType is null)
            {
                e.Accepted = false;
                return;
            }
            if (SelectedRoomType != null)
              if (hotelRoom.RoomType.RoomTypeName.ToString().Contains(SelectedRoomType.RoomTypeName.ToString(), StringComparison.OrdinalIgnoreCase))
                if (hotelRoom.BedNumber.ToString().Contains(_countBedText.ToString(), StringComparison.OrdinalIgnoreCase)) return;
            if (hotelRoom.RoomType.RoomTypeName.Contains(_countBedText.ToString(), StringComparison.OrdinalIgnoreCase)) return;
            e.Accepted = false;
        }

        private RoomType selectedRoomType;
        public RoomType SelectedRoomType
        {
            get => selectedRoomType;
            set
            {
                if (!Set(ref selectedRoomType, value)) return;
                _FilterHotelRooms.View.Refresh();
                SignalChanged();
            }
        }
        private Treatment selectedTreatment;
        public Treatment SelectedTreatment
        {
            get => selectedTreatment;
            set
            {
                Set(ref selectedTreatment, value);
                SignalChanged();
            }
        }
        private decimal totalPrice;
        public decimal TotalPrice
        {
            get => totalPrice;
            set
            {
                Set(ref totalPrice, value);
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
        private DateTime leaveDate;
        public DateTime LeaveDate
        {
            get { return leaveDate; }
            set
            {
                Set(ref leaveDate, value);
                SignalChanged();
            }
        }
        private DateTime arriveDate;
        public DateTime ArriveDate
        {
            get { return arriveDate; }
            set
            {
                Set(ref arriveDate, value);
                SignalChanged();
            }
        }
        private string countBedText;
        public string CountBedText
        {
            get { return countBedText; }
            set
            {
                if (!Set(ref countBedText, value)) return;
                _FilterHotelRooms.View.Refresh();
                SignalChanged();
            }
        }
        //private int selectedPersonCount;
        //public int SelectedPersonCount
        //{
        //    get => selectedPersonCount;
        //    set
        //    {
        //        Set(ref selectedPersonCount, value);
        //        SignalChanged();
        //    }
        //}
        private Meal selectedMeal;
        public Meal SelectedMeal
        {
            get => selectedMeal;
            set
            {
                Set(ref selectedMeal, value);
                SignalChanged();
            }
        }

        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public ObservableCollection<Treatment> Treatments { get; set; }
        public ObservableCollection<Meal> Meals { get; set; }
        public ObservableCollection<HotelRoom> HotelRooms { get; set; }

        public CustomCommand Calculate { get; set; }


        public ConstructorVM()
        {
            db = Db.GetDb();
            Treatments = new ObservableCollection<Treatment>(db.Treatments);
            SignalChanged("Treatments");
            Meals = new ObservableCollection<Meal>(db.Meals);
            SignalChanged("Meals");
            RoomTypes = new ObservableCollection<RoomType>(db.RoomTypes);
            SignalChanged("RoomTypes");
            HotelRooms = new ObservableCollection<HotelRoom>(db.HotelRooms);
            SignalChanged("HotelRooms");
            _FilterHotelRooms.Source = HotelRooms;
            _FilterHotelRooms.Filter += OnHotelRoomFiltred;
          

            Calculate = new CustomCommand(o =>
            {
                if (SelectedAdultCount == null)
                {
                    MessageBox.Show("Выберите количество персон!");
                    return;
                }
                if (SelectedChildCount == null)
                {
                    MessageBox.Show("Выберите количество персон!");
                    return;
                }
                TotalPriceCalc();
            });
        }
        
        public void TotalPriceCalc()
        {
            int adultcount = 1;
            int childcount = 1;
            decimal hotelroomprice = 0;
            double multiplier = 1;
            double fullDays = (LeaveDate - ArriveDate).TotalDays;
            for (int i = 0; i <= 8; i++)
                if ((string)SelectedAdultCount.Tag == i.ToString())
                    adultcount = i;

            for (int i = 0; i <= 8; i++)
                if ((string)SelectedChildCount.Tag == i.ToString())
                    childcount = i;

            if (adultcount > 1)
            {
                multiplier = adultcount * 0.6 + childcount * 0.3 * ((int)fullDays / 2);
            }
            if (SelectedRoomType != null)
            {
                hotelroomprice = SelectedRoomType.Price * int.Parse(CountBedText);
            }
            if (SelectedMeal != null && SelectedTreatment != null)
                TotalPrice = (SelectedMeal.Price + SelectedTreatment.Price + hotelroomprice) * (decimal)multiplier * ((decimal)fullDays / 2);
        }

        protected void SignalChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
