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
    public class OccupingConfirmVM : INotifyPropertyChanged
    {
        Db db;

        private Tour selectedTour;
        public Tour SelectedTour
        {
            get => selectedTour;
            set
            {
                Set(ref selectedTour, value);
                SignalChanged();
            }
        }
        
            private DateTime selectedTourFactArriveDate;
        public DateTime SelectedTourFactArriveDate
        {
            get => selectedTourFactArriveDate;
            set
            {
                Set(ref selectedTourFactArriveDate, value);
                SignalChanged();
            }
        }
        public ObservableCollection<Tour> Tours { get; set; }

        public CustomCommand Confirm { get; set; }

        public OccupingConfirmVM()
        {
            db = Db.GetDb();
            Tours = new ObservableCollection<Tour>(db.Tours);
            SignalChanged("Tours");

            selectedTourFactArriveDate = DateTime.Now;

            Confirm = new CustomCommand(o =>
            {
                if (SelectedTour.HotelRoom != null)
                {
                    SelectedTour.FactArriveDate = selectedTourFactArriveDate;
                    SelectedTour.HotelRoom.RoomStatus = db.RoomStatuses.FirstOrDefault(s => s.StatusName == "Занят");
                }
                SelectedTour.TourStatus = "occuped";
                try
                {
                    db.SaveChanges();
                    Tours = new ObservableCollection<Tour>(db.Tours);
                    SignalChanged("Tours");

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });

        }

        public OccupingConfirmVM(Tour selectedTour) : this()
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SignalChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
