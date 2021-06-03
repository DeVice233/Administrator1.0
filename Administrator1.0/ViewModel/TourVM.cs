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
    class TourVM : INotifyPropertyChanged
    {
        public Window window { get; set; }
        Db db;
        public ComboBoxItem SelectedType { get; set; }

        private readonly CollectionViewSource _FilterTours = new CollectionViewSource();

        public ICollectionView FilterTours => _FilterTours?.View;

        private void OnClientFiltred(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Tour tour))
            {
                e.Accepted = false;
                return;
            }

            var filter_text = _TourFilterText;
            if (string.IsNullOrWhiteSpace(filter_text))
                return;
            if (tour.ArriveDate.ToString() is null || tour.LeaveDate.ToString() is null
                || tour.HotelRoom is null)
            {
                e.Accepted = false;
                return;
            }
            if ((string)SelectedType.Tag == "0")
                if (tour.ArriveDate.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "1")
                if (tour.LeaveDate.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "2")
                if (tour.HotelRoom.RoomNumber.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "3")
                if (tour.HotelRoom.RoomStatus.StatusName.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;

            e.Accepted = false;
        }

        public TourAdvanced tourAdvanced = new TourAdvanced();

        public CustomCommand OpenTourAdvanced { get; set; }
        public CustomCommand OpenNewTourAdvanced { get; set; }
        public CustomCommand DeleteTour { get; set; }

        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<HotelRoom> HotelRooms { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }

        private string _TourFilterText;
        public string TourFilterText
        {
            get => _TourFilterText;
            set
            {
                if (SelectedType == null)
                {
                    MessageBox.Show("Выберите тип поиска!");
                    return;
                }
                if (!Set(ref _TourFilterText, value)) return;
                _FilterTours.View.Refresh();
            }
        }

        private double fullDays;
        public double FullDays
        {
            get => fullDays;
            set
            {
                Set(ref fullDays, value);
                SignalChanged();
            }
        }

        private Tour selectedTour = new Tour { };
        public Tour SelectedTour
        {
            get => selectedTour;
            set
            {
                Set(ref selectedTour, value);
                SignalChanged();
            }
        }
        private int selectedTourClientCount;
        public int SelectedTourClientCount
        {
            get => selectedTourClientCount;
            set
            {
                Set(ref selectedTourClientCount, value);
                SignalChanged();
            }
        }
       
        public TourVM()
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
            _FilterTours.Source = Tours;
            _FilterTours.Filter += OnClientFiltred;
            FullDays = (SelectedTour.ArriveDate - SelectedTour.LeaveDate).TotalDays;
            SignalChanged("FullDays");
            SignalChanged("fullDays");
            OpenTourAdvanced = new CustomCommand(o =>
            {
                window = new TourAdvanced(SelectedTour);
                window.ShowDialog();

            });
            DeleteTour = new CustomCommand(o =>
            {
                if (SelectedTour != null)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Предупреждение", "Удалить выбранную путевку?",
                         MessageBoxButton.OKCancel, MessageBoxImage.Warning
                        );
                    if (messageBoxResult == MessageBoxResult.OK)
                    {
                        try
                        {
                            db.Tours.Remove(SelectedTour);
                            db.SaveChanges();
                            Tours = new ObservableCollection<Tour>(db.Tours);
                            SignalChanged("Tours");
                        }
                        catch(Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                    }
                }
            });
            OpenNewTourAdvanced = new CustomCommand(o =>
            {
            MessageBoxResult dialogResult = MessageBox.Show(
                   "Создать новую путевку?",
                   "Сообщение",
                   MessageBoxButton.OKCancel,
                   MessageBoxImage.Question
                   );
            if (dialogResult == MessageBoxResult.OK)
            {
                var tour = new Tour {  };
                    db.Tours.Add(tour);
                    SelectedTour = tour;
                    Tours = new ObservableCollection<Tour>(db.Tours);
                    SignalChanged("Tours");
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
                    window = new TourAdvanced(SelectedTour);
                    window.ShowDialog();
                }
            });
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
