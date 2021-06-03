using Administrator1._0.DBEntities;
using Administrator1._0.View;
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

            string status;
            status = StatusConvert(tour.TourStatus);

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
            if ((string)SelectedType.Tag == "4")
                if (status.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "5")
                if (tour.Customer.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            e.Accepted = false;
        }

        public TourAdvanced tourAdvanced = new TourAdvanced();

        public CustomCommand OpenTourAdvanced { get; set; }
        public CustomCommand OpenNewTourAdvanced { get; set; }
        public CustomCommand DeleteTour { get; set; }
        public CustomCommand Occuping { get; set; }
        public CustomCommand UnOccuping { get; set; }
        public CustomCommand MigrationOpen { get; set; }
        public CustomCommand CancelOccuping { get; set; }
        public CustomCommand CancelUnOccuping { get; set; }
        public CustomCommand PrintTourInfo { get; set; }
        public CustomCommand Refresh { get; set; }

        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<HotelRoom> HotelRooms { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<Treatment> Treatments { get; set; }
        public ObservableCollection<Meal> Meals { get; set; }

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
            Treatments = new ObservableCollection<Treatment>(db.Treatments);
            SignalChanged("Treatments");
            Meals = new ObservableCollection<Meal>(db.Meals);
            SignalChanged("Meals");
            _FilterTours.Source = Tours;
            _FilterTours.Filter += OnClientFiltred;
         
          
            PrintTourInfo = new CustomCommand(o =>
            {
                Print();
            });
            Occuping = new CustomCommand(o =>
            {
                if (SelectedTour == null)
                    MessageBox.Show("Выберите путевку");
                else
                {
                    window = new OccupingConfirm(SelectedTour);
                    window.ShowDialog();
                }
            });
            CancelUnOccuping = new CustomCommand(o =>
            {
                if (SelectedTour == null)
                    MessageBox.Show("Выберите путевку");
                else
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Отменить выселение?", "Предупреждение",
                         MessageBoxButton.OKCancel, MessageBoxImage.Warning
                        );
                    if (messageBoxResult == MessageBoxResult.OK)
                    {
                        SelectedTour.TourStatus = "occuped";
                        SelectedTour.FactLeaveDate = null;
                        SelectedTour.HotelRoom.RoomStatus = db.RoomStatuses.FirstOrDefault(s => s.StatusName == "Занят");
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
                    }
                 }
            });
            CancelOccuping = new CustomCommand(o =>
            {
                if (SelectedTour == null)
                    MessageBox.Show("Выберите путевку");
                else
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Отменить заселение?", "Предупреждение",
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning
                       );
                    if (messageBoxResult == MessageBoxResult.OK)
                    {
                        SelectedTour.TourStatus = "notstated";
                        SelectedTour.FactArriveDate = null;
                        SelectedTour.HotelRoom.RoomStatus = db.RoomStatuses.FirstOrDefault(s => s.StatusName == "Забронирован");
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
                    }
                }
            });
            //MigrationOpen = new CustomCommand(o =>
            //{
            //    if (SelectedTour == null)
            //        MessageBox.Show("Выберите путевку");
            //    else
            //    {
            //        window = new Migration(SelectedTour);
            //        window.ShowDialog();
            //    }
            //});
            UnOccuping = new CustomCommand(o =>
            {
                if (SelectedTour == null)
                    MessageBox.Show("Выберите путевку");
                else
                {
                    window = new UnOccupingConfirm(SelectedTour);
                    window.ShowDialog();
                }

            });

            OpenTourAdvanced = new CustomCommand(o =>
            {
                window = new TourAdvanced(SelectedTour);
                window.ShowDialog();

            });
            DeleteTour = new CustomCommand(o =>
            {
                if (SelectedTour != null)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Удалить выбранную путевку?", "Предупреждение",
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

        public void FullDaysCalc()
        {
            FullDays = (SelectedTour.LeaveDate - SelectedTour.ArriveDate).TotalDays;
            SignalChanged("FullDays");
        }
        public void Print()
        {
            string status = SelectedTour.TourStatus;
            status = StatusConvert(status);
            if (SelectedTour == null)
                MessageBox.Show("Выберите путевку");
            else
            {

                var workbook = new Workbook();
                var sheet = workbook.Worksheets[0];
                sheet.Range["E1"].Value = DateTime.Now.ToShortDateString();
                sheet.Range["E3"].Value = $"Заказчик - ";
                sheet.Range["F3"].Value = $"{SelectedTour.Customer}";

                sheet.Range["E4"].Value = $"Кол-во полных дней - ";
                sheet.Range["F4"].Value = $"{FullDays}";

                sheet.Range["A1"].Value = $"Номер путевки: ";
                sheet.Range["B1"].Value = $"{SelectedTour.Id}";

                sheet.Range["A3"].Value = $"Текущий статус путевки: ";
                sheet.Range["C3"].Value = $"{status}";
                sheet.Range["A3:B3"].Merge();

                sheet.Range["A4"].Value = $"Плановая дата заезда: ";
                sheet.Range["C4"].Value = $"{SelectedTour.ArriveDate.ToShortDateString()}";
                sheet.Range["A4:B4"].Merge();

                sheet.Range["A5"].Value = $"Фактическая дата заезда: ";
                sheet.Range["C5"].Value = $"{SelectedTour.FactArriveDate}";
                sheet.Range["A5:B5"].Merge();

                sheet.Range["A6"].Value = $"Плановая дата отъезда: ";
                sheet.Range["C6"].Value = $"{SelectedTour.LeaveDate.ToShortDateString()}";
                sheet.Range["A6:B6"].Merge();



                sheet.Range["A7"].Value = $"Фактическая дата отъезда: ";
                sheet.Range["C7"].Value = $"{SelectedTour.FactLeaveDate}";
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

                sheet.Range["A15"].Value = $"Номер:";
                sheet.Range["B15"].Value = $"Имя:";
                sheet.Range["C15"].Value = $"Фамилия:";
                sheet.Range["D15"].Value = $"Отчество:";
                sheet.Range["E15"].Value = $"Пол:";
                sheet.Range["F15"].Value = $"Дата рождения:";
                sheet.Range["G15"].Value = $"Номер паспорта:";
                sheet.Range["H15"].Value = $"Серия паспорта:";

                int index = 16;
                int count = 1;
                
                foreach (var client in SelectedTour.Clients )
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

                sheet.Range[$"A{index}"].Value = $"Оплаты: ";
                sheet.Range[$"A{index}:H{index}"].Merge();
                sheet.Range[$"A{index}"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                index++;
                sheet.Range[$"A{index}"].Value = $"Тип Лечения: ";
                sheet.Range[$"B{index}"].Value = $"{SelectedTour.Treatment.TreatmentName}";
                sheet.Range[$"C{index}"].Value = $"Цена: ";
                sheet.Range[$"D{index}"].Value = $"{SelectedTour.Treatment.Price}";
                index++;
                sheet.Range[$"А{index}"].Value = $"Тип Питания: ";
                sheet.Range[$"B{index}"].Value = $"{SelectedTour.Meal.MealName}";
                sheet.Range[$"C{index}"].Value = $"Цена: ";
                sheet.Range[$"D{index}"].Value = $"{SelectedTour.Meal.Price}";
                index++;
               
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

            if (status == "occuped")
            {
                status = "Заселен";
            }
            if (status == "unoccuped")
            {
                status = "Выселен";
            }
            if (status == null)
            {
                status = "Новый";
            }
            return status;
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
