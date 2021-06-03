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
    class UnOccupingConfirmVM : INotifyPropertyChanged
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

        private DateTime selectedTourFactLeaveDate;
        public DateTime SelectedTourFactLeaveDate
        {
            get => selectedTourFactLeaveDate;
            set
            {
                Set(ref selectedTourFactLeaveDate, value);
                SignalChanged();
            }
        }
        public ObservableCollection<Tour> Tours { get; set; }

        public CustomCommand Confirm { get; set; }

        public UnOccupingConfirmVM()
        {
            db = Db.GetDb();
            Tours = new ObservableCollection<Tour>(db.Tours);
            SignalChanged("Tours");

            selectedTourFactLeaveDate = DateTime.Now;

            Confirm = new CustomCommand(o =>
            {
                SelectedTour.TourStatus = "unoccuped";
                if (SelectedTour.HotelRoom != null)
                {
                    SelectedTour.FactLeaveDate = selectedTourFactLeaveDate;
                    SelectedTour.HotelRoom.RoomStatus = db.RoomStatuses.FirstOrDefault(s => s.StatusName == "Свободен");
                }
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

        public UnOccupingConfirmVM(Tour selectedTour) : this()
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
