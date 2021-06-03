using Administrator1._0.DBEntities;
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
    public class EtcVM : INotifyPropertyChanged
    {
        public Page CurrentPage { get; set; }
        Db db;
        private Meal selectedMeal;
        public Meal SelectedMeal
        {
            get => selectedMeal;
            set
            {
                selectedMeal = value;
                SignalChanged();
            }
        }
        
        private Treatment selectedTreatment;
        public Treatment SelectedTreatment
        {
            get => selectedTreatment;
            set
            {
                selectedTreatment = value;
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
        private Service selectedService;
        public Service SelectedService
        {
            get => selectedService;
            set
            {
                selectedService = value;
                SignalChanged();
            }
        }

        public ObservableCollection<Meal> Meals { get; set; }
        public ObservableCollection<Treatment> Treatments { get; set; }
        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public ObservableCollection<RoomStatus> RoomStatuses { get; set; }
        public ObservableCollection<Service> Services { get; set; }

        public CustomCommand AddService { get; set; }
        public CustomCommand SaveService { get; set; }
        public CustomCommand RemoveService { get; set; }
        public CustomCommand AddMeal { get; set; }
        public CustomCommand SaveMeal { get; set; }
        public CustomCommand RemoveMeal { get; set; }
        public CustomCommand AddTreatment { get; set; }
        public CustomCommand SaveTreatment { get; set; }
        public CustomCommand RemoveTreatment { get; set; }
        public CustomCommand AddRoomStatus { get; set; }
        public CustomCommand SaveRoomStatus { get; set; }
        public CustomCommand RemoveRoomStatus { get; set; }
        public CustomCommand AddRoomType { get; set; }
        public CustomCommand SaveRoomType { get; set; }
        public CustomCommand RemoveRoomType { get; set; }


        public EtcVM()
        {
            db = Db.GetDb();
            Meals = new ObservableCollection<Meal>(db.Meals);
            SignalChanged("Meals");
            Treatments = new ObservableCollection<Treatment>(db.Treatments);
            SignalChanged("Treatmets");
            RoomStatuses = new ObservableCollection<RoomStatus>(db.RoomStatuses);
            SignalChanged("RoomStatuses");
            RoomTypes = new ObservableCollection<RoomType>(db.RoomTypes);
            SignalChanged("RoomTypes");
            Services = new ObservableCollection<Service>(db.Services);
            SignalChanged("Services");
           
            AddMeal = new CustomCommand(o =>
            {
                var meal = new Meal { MealName = "Название", Price = 00 };
                db.Meals.Add(meal);
                SelectedMeal = meal;
                Meals = new ObservableCollection<Meal>(db.Meals);
                SignalChanged("Meals");
            });
            SaveMeal = new CustomCommand(o =>
            {
                try
                {
                    db.SaveChanges();
                    Meals = new ObservableCollection<Meal>(db.Meals);
                    SignalChanged("Meals");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            RemoveMeal = new CustomCommand(o =>
            {
                db.Meals.Remove(SelectedMeal);
                try
                {
                    db.SaveChanges();
                    Meals = new ObservableCollection<Meal>(db.Meals);
                    SignalChanged("Meals");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            
            AddService = new CustomCommand(o =>
            {
                var service = new Service { ServiceName = "Название", Price = 00, Duration = "0 мин." };
                db.Services.Add(service);
                SelectedService = service;
                Services = new ObservableCollection<Service>(db.Services);
                SignalChanged("Services");
            });
            SaveService = new CustomCommand(o =>
            {
                try
                {
                    db.SaveChanges();
                    Services = new ObservableCollection<Service>(db.Services);
                    SignalChanged("Services");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            RemoveService = new CustomCommand(o =>
            {
                db.Services.Remove(SelectedService);
                try
                {
                    db.SaveChanges();
                    Services = new ObservableCollection<Service>(db.Services);
                    SignalChanged("Services");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            AddTreatment = new CustomCommand(o =>
            {
                var treatment = new Treatment { TreatmentName = "Название", Price = 00 };
                db.Treatments.Add(treatment);
                SelectedTreatment = treatment;
                Treatments = new ObservableCollection<Treatment>(db.Treatments);
                SignalChanged("Treatments");
            });
            SaveTreatment = new CustomCommand(o =>
            {
                try
                {
                    db.SaveChanges();
                    Treatments = new ObservableCollection<Treatment>(db.Treatments);
                    SignalChanged("Treatments");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            RemoveTreatment = new CustomCommand(o =>
            {
                try
                {
                    db.Treatments.Remove(SelectedTreatment);
                    db.SaveChanges();
                    Treatments = new ObservableCollection<Treatment>(db.Treatments);
                    SignalChanged("Treatments");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            AddRoomStatus = new CustomCommand(o =>
            {
                var roomstatus = new RoomStatus { StatusName = "Название" };
                db.RoomStatuses.Add(roomstatus);
                SelectedRoomStatus = roomstatus;
                RoomStatuses = new ObservableCollection<RoomStatus>(db.RoomStatuses);
                SignalChanged("RoomStatuses");
            });
            SaveRoomStatus = new CustomCommand(o =>
            {
                try
                {
                    db.SaveChanges();
                    RoomStatuses = new ObservableCollection<RoomStatus>(db.RoomStatuses);
                    SignalChanged("RoomStatuses");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            RemoveRoomStatus = new CustomCommand(o =>
            {
                db.RoomStatuses.Remove(SelectedRoomStatus);
            });
            AddRoomType = new CustomCommand(o =>
            {
                var roomtype = new RoomType { RoomTypeName = "Название" };
                db.RoomTypes.Add(roomtype);
                SelectedRoomType = roomtype ;
                RoomTypes = new ObservableCollection<RoomType>(db.RoomTypes);
                SignalChanged("RoomTypes");
            });
            SaveRoomType = new CustomCommand(o =>
            {
                try
                {
                    db.SaveChanges();
                    RoomTypes = new ObservableCollection<RoomType>(db.RoomTypes);
                    SignalChanged("RoomTypes");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            RemoveRoomType = new CustomCommand(o =>
            {
                db.RoomTypes.Remove(SelectedRoomType);
            });
        }

        protected void SignalChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
