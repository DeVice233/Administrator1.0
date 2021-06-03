using Administrator1._0.DBEntities;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
        
        private decimal totalPrice ;
        public decimal TotalPrice
        {
            get => totalPrice; 
            set
            {
                Set(ref totalPrice, value);
                SignalChanged();
            }
        }
        private int childCount;
        public int ChildCount
        {
            get => childCount;
            set
            {
                Set(ref childCount, value);
                SignalChanged();
            }
        }
        private int adultCount;
        public int AdultCount
        {
            get => adultCount;
            set
            {
                Set(ref adultCount, value);
                SignalChanged();
            }
        }
        private decimal orderedServicesCash;
        public decimal OrderedServicesCash
        {
            get => orderedServicesCash;
            set
            {
                Set(ref orderedServicesCash, value);
                SignalChanged();
            }
        }
        private Order selectedOrder;
        public Order SelectedOrder
        {
            get => selectedOrder;
            set
            {
                Set(ref selectedOrder, value);
                SignalChanged();
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

        private readonly CollectionViewSource _FilterOrders = new CollectionViewSource();
        public ICollectionView FilterOrders => _FilterOrders?.View;
        private void OnOrderFiltred(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Order order))
            {
                e.Accepted = false;
                return;
            }

            if (order.Service is null || order.Client.Tour is null ||
                order.Client is null)
            {
                e.Accepted = false;
                return;
            }
           
            
            if (order.Client.Tour.Id.ToString().Contains(SelectedTour.Id.ToString()))
            {
                SelectedTourOrders.Add(order);
                if (order.Status == "done")
                    return;
                if (order.Status == null)
                    return;
                if (order.Status == "canceled")
                    return;
                OrderedServicesCash += order.Service.Price;
                return;
            }

            e.Accepted = false;
        }

        private Tour selectedTour = new Tour { Clients = new List<Client>()};
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (!Set(ref selectedTour, value)) return;
                _FilterOrders.View.Refresh();
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
        private decimal hotelRoomPrice;
        public decimal HotelRoomPrice
        {
            get => hotelRoomPrice;
            set
            {
                Set(ref hotelRoomPrice, value);
                SignalChanged();
            }
        }
        private decimal mealPrice;
        public decimal MealPrice
        {
            get => mealPrice;
            set
            {
                Set(ref mealPrice, value);
                SignalChanged();
            }
        }
        private decimal treatmentPrice;
        public decimal TreatmentPrice
        {
            get => treatmentPrice;
            set
            {
                Set(ref treatmentPrice, value);
                SignalChanged();
            }
        }
        private decimal cashToPay;
        public decimal CashToPay
        {
            get => cashToPay;
            set
            {
                Set(ref cashToPay, value);
                SignalChanged();
            }
        }
        public CustomCommand TourSave { get; set; }
        public CustomCommand AddSelectedRoom { get; set; }
        public CustomCommand AddClient { get; set; }
        public CustomCommand RemoveClient { get; set; }
        public CustomCommand DoCustomer { get; set; }
        public CustomCommand RemoveCustomer { get; set; }
        public CustomCommand Pay { get; set; }
        public CustomCommand PrintTourInfo { get; set; }

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

        public ObservableCollection<Treatment> Treatments { get; set; }
        public ObservableCollection<Meal> Meals { get; set; }
        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<HotelRoom> HotelRooms { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<RoomStatus> RoomStatuses { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<Service> Services { get; set; }

        public ObservableCollection<Client> SelectedTourClients
        {
            get => new ObservableCollection<Client>(SelectedTour.Clients);
        }
        public List<Order> SelectedTourOrders { get; set; }
       
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
            Treatments = new ObservableCollection<Treatment>(db.Treatments);
            SignalChanged("Treatments");
            Meals = new ObservableCollection<Meal>(db.Meals);
            SignalChanged("Meals");
            Orders = new ObservableCollection<Order>(db.Orders);
            SignalChanged("Orders");
            Services = new ObservableCollection<Service>(db.Services);
            SignalChanged("Services");

            AgeCalc();

            SelectedTourOrders = new List<Order>();

            _FilterHotelRooms.Source = HotelRooms;
            _FilterHotelRooms.Filter += OnHotelRoomFiltred;
            _FilterClients.Source = Clients;
            _FilterClients.Filter += OnClientFiltred;
            _FilterOrders.Source = Orders;
            _FilterOrders.Filter += OnOrderFiltred;

            PrintTourInfo = new CustomCommand(o =>
            {
                Print();
            });
            DoCustomer = new CustomCommand(o =>
            {
                SelectedTour.Customer = SelectedClient.FirstName + " " + SelectedClient.SecondName + " " + SelectedClient.FatherName;
                db.SaveChanges();
            });
            Pay = new CustomCommand(o =>
            {
                SelectedTour.Payment = SelectedTour.Payment + CashToPay;
                TotalPriceCalc();
                db.SaveChanges();
            });
            RemoveCustomer = new CustomCommand(o =>
            {
                SelectedTour.Customer = " ";
            });
            TourSave = new CustomCommand(o =>
            {
                try
                {
                    db.SaveChanges();
                    Tours = new ObservableCollection<Tour>(db.Tours);
                    SignalChanged("Tours");
                    TotalPriceCalc();
                    FullDaysCalc();
                    AgeCalc();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
            AddSelectedRoom = new CustomCommand(o =>
            {
                if (SelectedHotelRoom.RoomStatus != db.RoomStatuses.FirstOrDefault(s => s.StatusName == "Свободен"))
                    MessageBox.Show("Выберите другой номер");
                else
                {
                    SelectedTour.HotelRoom = SelectedHotelRoom;
                    SelectedHotelRoom.RoomStatus = db.RoomStatuses.FirstOrDefault(s => s.StatusName == "Забронирован");
                    db.SaveChanges();
                    Tours = new ObservableCollection<Tour>(db.Tours);
                    SignalChanged("Tours");
                    TotalPriceCalc();
                    AgeCalc();
                }
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
        public void FullDaysCalc()
        {
            FullDays = (SelectedTour.LeaveDate - SelectedTour.ArriveDate).TotalDays;
            SignalChanged("FullDays");
        }
       
        public void AgeCalc()
        {
            int childcount = 0;
            int adultcount = 0;
            foreach (Client client in SelectedTourClients)
            {
                string a;
                a = Convert.ToString(client.Birthday.ToShortDateString());
                var date = DateTime.ParseExact(a, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                var age = DateTime.Now.Year - date.Year;
                if (DateTime.Now.Month < date.Month ||
                   (DateTime.Now.Month == date.Month && DateTime.Now.Day < date.Day)) age--;
                if (age >= 18)
                    adultcount++;
                else
                    childcount++;
            }
            AdultCount = adultcount;
            ChildCount = childcount;
        }


        public void TotalPriceCalc()
        {
            int childcount = 0;
            int adultcount = 0;
            foreach (Client client in SelectedTourClients)
            {
                string a;
                a = Convert.ToString(client.Birthday.ToShortDateString());
                var date = DateTime.ParseExact(a, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                var age = DateTime.Now.Year - date.Year;
                if (DateTime.Now.Month < date.Month ||
                   (DateTime.Now.Month == date.Month && DateTime.Now.Day < date.Day)) age--;
                if (age >= 18)
                    adultcount++;
                else
                    childcount++;
            }
            decimal hotelroomprice = 0;
            double multiplier = 1;
            if (SelectedTourClients.Count > 1)
            {
                multiplier = adultcount * 0.8 + childcount * 0.4;
            }
            if (SelectedTour.HotelRoom != null)
            {
                hotelroomprice = SelectedTour.HotelRoom.RoomType.Price * SelectedTour.HotelRoom.BedNumber;
            }
            if (SelectedTour.Meal != null && SelectedTour.Treatment != null)
            {
                SelectedTour.Price = (SelectedTour.Meal.Price + SelectedTour.Treatment.Price + hotelroomprice) * (decimal)multiplier * ((decimal)FullDays) + OrderedServicesCash;
                HotelRoomPrice = hotelroomprice * ((decimal)FullDays) * (decimal)multiplier;
                TreatmentPrice = SelectedTour.Treatment.Price * ((decimal)FullDays) * (decimal)multiplier;
                MealPrice = SelectedTour.Meal.Price * ((decimal)FullDays) * (decimal)multiplier;
                SelectedTour.Debt = SelectedTour.Price;
                if (SelectedTour.Payment == null)
                    SelectedTour.Payment = 0;
                SelectedTour.Debt = SelectedTour.Price - SelectedTour.Payment;
                
            }
        }
        public void Print()
        {
            string status = SelectedTour.TourStatus;
            status = StatusConvert(status);
            if (SelectedTour == null)
                MessageBox.Show("Выберите путевку");
            else
            {
                string factarrive;
                string factleave;
                if (SelectedTour.FactArriveDate == null)
                {
                    factarrive = "Не указано";
                }
                else
                    factarrive = SelectedTour.FactArriveDate.ToString();
                if (SelectedTour.FactLeaveDate == null)
                {
                    factleave = "Не указано";
                }
                else
                    factleave = SelectedTour.FactLeaveDate.ToString();
                var workbook = new Workbook();
                var sheet = workbook.Worksheets[0];
                sheet.Range["E1"].Value = DateTime.Now.ToShortDateString();
                sheet.Range["E3"].Value = $"Заказчик - ";
                sheet.Range["F3"].Value = $"{SelectedTour.Customer}";

                sheet.Range["E4"].Value = $"Кол-во полных дней - ";
                sheet.Range["F4"].Value = $"{FullDays}";

                sheet.Range["E6"].Value = $"Кол-во взрослых - ";
                sheet.Range["F6"].Value = $"{AdultCount}";

                sheet.Range["E7"].Value = $"Кол-во детей - ";
                sheet.Range["F7"].Value = $"{ChildCount}";

                sheet.Range["A1"].Value = $"Номер путевки: ";
                sheet.Range["B1"].Value = $"{SelectedTour.Id}";

                sheet.Range["A3"].Value = $"Текущий статус путевки: ";
                sheet.Range["C3"].Value = $"{status}";
                sheet.Range["A3:B3"].Merge();

                sheet.Range["A4"].Value = $"Плановая дата заезда: ";
                sheet.Range["C4"].Value = $"{SelectedTour.ArriveDate.ToShortDateString()}";
                sheet.Range["A4:B4"].Merge();

                sheet.Range["A5"].Value = $"Фактическая дата заезда: ";
                sheet.Range["C5"].Value = $"{factarrive}";
                sheet.Range["A5:B5"].Merge();

                sheet.Range["A6"].Value = $"Плановая дата отъезда: ";
                sheet.Range["C6"].Value = $"{SelectedTour.LeaveDate.ToShortDateString()}";
                sheet.Range["A6:B6"].Merge();



                sheet.Range["A7"].Value = $"Фактическая дата отъезда: ";
                sheet.Range["C7"].Value = $"{factleave}";
                sheet.Range["A7:B7"].Merge();

                sheet.Range["A9"].Value = $"Номер комнаты: ";
                sheet.Range["C9"].Value = $"{SelectedTour.HotelRoom.RoomNumber}";
                sheet.Range["A9:B9"].Merge();

                sheet.Range["A10"].Value = $"Категория комнаты: ";
                sheet.Range["C10"].Value = $"{SelectedTour.HotelRoom.RoomType.RoomTypeName}";
                sheet.Range["A10:B10"].Merge();

                sheet.Range["A11"].Value = $"Кол-во мест в номере: ";
                sheet.Range["C11"].Value = $"{SelectedTour.HotelRoom.BedNumber}";
                sheet.Range["A11:B11"].Merge();

                sheet.Range["A14"].Value = $"Клиенты: ";
                sheet.Range["A14:H14"].Merge();
                sheet.Range["A14"].Style.HorizontalAlignment = HorizontalAlignType.Center;

                sheet.Range["A15"].Value = $"№";
                sheet.Range["B15"].Value = $"Имя:";
                sheet.Range["C15"].Value = $"Фамилия:";
                sheet.Range["D15"].Value = $"Отчество:";
                sheet.Range["E15"].Value = $"Пол:";
                sheet.Range["F15"].Value = $"Дата рождения:";
                sheet.Range["G15"].Value = $"Номер паспорта:";
                sheet.Range["H15"].Value = $"Серия паспорта:";

                int index = 16;
                int count = 1;

                foreach (var client in SelectedTour.Clients)
                {
                    sheet.Range[$"A{index}"].NumberValue = count++;
                    sheet.Range[$"B{index}"].Value = client.FirstName;
                    sheet.Range[$"C{index}"].Value = client.SecondName;
                    sheet.Range[$"D{index}"].Value = client.FatherName;
                    sheet.Range[$"E{index}"].Value = client.Sex;
                    sheet.Range[$"F{index}"].Value = client.Birthday.ToShortDateString();
                    sheet.Range[$"G{index}"].Value = client.PassportNumber.ToString();
                    sheet.Range[$"H{index}"].Value = client.PassportSerias.ToString();
                    index++;
                }
                sheet.Range[$"A15:H{index - 1}"].BorderInside(LineStyleType.Thin);
                sheet.Range[$"A15:H{index - 1}"].BorderAround(LineStyleType.Medium);
                index++;
                sheet.Range[$"A{index}"].Value = $"Оплаты: ";
                sheet.Range[$"A{index}:H{index}"].Merge();
                sheet.Range[$"A{index}"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                index++;
                sheet.Range[$"A{index}"].Value = $"Тип Лечения: ";
                sheet.Range[$"B{index}"].Value = $"{SelectedTour.Treatment.TreatmentName}";
                sheet.Range[$"C{index}"].Value = $"Цена: ";
                sheet.Range[$"D{index}"].Value = $"{TreatmentPrice}";
                index++;
                sheet.Range[$"A{index}"].Value = "Тип Питания:";
                sheet.Range[$"B{index}"].Value = $"{SelectedTour.Meal.MealName}";
                sheet.Range[$"C{index}"].Value = $"Цена: ";
                sheet.Range[$"D{index}"].Value = $"{MealPrice}";
                index++;
                sheet.Range[$"A{index}"].Value = "Тип номера:";
                sheet.Range[$"B{index}"].Value = $"{SelectedTour.HotelRoom.RoomType.RoomTypeName}";
                sheet.Range[$"C{index}"].Value = $"Цена: ";
                sheet.Range[$"D{index}"].Value = $"{HotelRoomPrice}";
                sheet.Range[$"A{index - 2}:D{index}"].BorderInside(LineStyleType.Thin);
                sheet.Range[$"A{index - 2}:D{index}"].BorderAround(LineStyleType.Medium);
                index++;
                index++;
                sheet.Range[$"A{index}"].Value = "Сумма:";
                sheet.Range[$"B{index}"].Value = $"{SelectedTour.Price}";
                index++;
                sheet.Range[$"A{index}"].Value = "Оплата:";
                sheet.Range[$"B{index}"].Value = $"{SelectedTour.Payment}";
                index++;
                sheet.Range[$"A{index}"].Value = "Долг:";
                sheet.Range[$"B{index}"].Value = $"{SelectedTour.Debt}";
                index++;
                index++;
                sheet.Range[$"A{index}"].Value = $"Заказанные услуги: ";
                sheet.Range[$"A{index}:H{index}"].Merge();
                sheet.Range[$"A{index}"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                index++;
                sheet.Range[$"A{index}"].Value = $"№";
                sheet.Range[$"B{index}"].Value = $"Название:";
                sheet.Range[$"C{index}"].Value = $"Дата:";
                sheet.Range[$"D{index}"].Value = $"Длительность:";
                sheet.Range[$"E{index}"].Value = $"Цена:";
                sheet.Range[$"F{index}"].Value = $"Имя клиента:";
                sheet.Range[$"G{index}"].Value = $"Фамилия клиента:";
                sheet.Range[$"H{index}"].Value = $"Дата рождения:";
               
                index++;
                int seccount = 1;
                decimal ordersprice = 0;
                foreach (var order in SelectedTourOrders)
                {
                    sheet.Range[$"A{index}"].NumberValue = seccount++;
                    sheet.Range[$"B{index}"].Value = order.Service.ServiceName;
                    sheet.Range[$"C{index}"].Value = order.Date.ToShortDateString();
                    sheet.Range[$"D{index}"].Value = order.Service.Duration;
                    sheet.Range[$"E{index}"].Value = order.Service.Price.ToString();
                    sheet.Range[$"F{index}"].Value = order.Client.FirstName;
                    sheet.Range[$"G{index}"].Value = order.Client.SecondName;
                    sheet.Range[$"H{index}"].Value = order.Client.Birthday.ToShortDateString();
                    ordersprice += order.Service.Price;
                    sheet.Range[$"A{index - 1}:H{index}"].BorderInside(LineStyleType.Thin);
                    sheet.Range[$"A{index - 1}:H{index}"].BorderAround(LineStyleType.Medium);
                    index++;
                }
                index++;
                sheet.Range[$"A{index}"].Value = $"Общая сумма заказанных услуг:";
                sheet.Range[$"A{index}:C{index}"].Merge();
                sheet.Range[$"D{index}"].Value = $"{ordersprice}";

                sheet.AllocatedRange.AutoFitColumns();

                workbook.SaveToFile("text.xls");
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/text.xls")
                {
                    UseShellExecute = true
                };
                p.Start();
            }
        }

        public string StatusConvert(string status)
        {

            if (SelectedTour.TourStatus == "occuped")
            {
                status = "Заселен";
            }
            if (SelectedTour.TourStatus == "unoccuped")
            {
                status = "Выселен";
            }
            if (SelectedTour.TourStatus == null)
            {
                status = "Новый";
            }
            return status;
        }
       
        public TourAdvancedVM(Tour selectedTour) : this()
        {
            SelectedTour = selectedTour;
            SelectedTour.Clients = db.Tours.Where(s => s == selectedTour).SelectMany(b => b.Clients).ToList();
            FullDaysCalc();
            TotalPriceCalc();
            AgeCalc();
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
            Treatments = new ObservableCollection<Treatment>(db.Treatments);
            SignalChanged("Treatments");
            Meals = new ObservableCollection<Meal>(db.Meals);
            SignalChanged("Meals");
            Orders = new ObservableCollection<Order>(db.Orders);
            SignalChanged("Orders");
            Services = new ObservableCollection<Service>(db.Services);
            SignalChanged("Services");
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
