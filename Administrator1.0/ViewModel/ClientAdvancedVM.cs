using Administrator1._0.DBEntities;
using Administrator1._0.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Administrator1._0.ViewModel
{
    public class ClientAdvancedVM : INotifyPropertyChanged
    {

        Db db;
        public Window window { get; set; }

        private Client selectedClient = new Client { Orders = new List<Order>() };
        public Client SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                SignalChanged();
            }
        }
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
        private Order selectedOrder;
        public Order SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
                SignalChanged();
            }
        }
        private int selectedClientAge;
        public int SelectedClientAge
        {
            get => selectedClientAge;
            set
            {
                selectedClientAge = value;
                SignalChanged();
            }
        }

        public ObservableCollection<Order> SelectedClientOrders
        {
            get => new ObservableCollection<Order>(SelectedClient.Orders);

        }

        public ObservableCollection<Treatment> Treatments { get; set; }
        public ObservableCollection<Meal> Meals { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<Service> Services { get; set; }

        public CustomCommand ClientSave { get; set; }
        public CustomCommand ClientClose { get; set; }

        public ClientAdvancedVM()
        {
            db = Db.GetDb();
            Clients = new ObservableCollection<Client>(db.Clients);
            SignalChanged("Clients");
            Services = new ObservableCollection<Service>(db.Services);
            SignalChanged("Services");
            Orders = new ObservableCollection<Order>(db.Orders);
            SignalChanged("Orders");
            Treatments = new ObservableCollection<Treatment>(db.Treatments);
            SignalChanged("Treatments");
            Meals = new ObservableCollection<Meal>(db.Meals);
            SignalChanged("Meals");


            

            ClientClose = new CustomCommand(o =>
            {
              
            });
            ClientSave = new CustomCommand(o =>
            {
                try
                {
                    AgeCalculate();
                    db.SaveChanges();
                    db = Db.GetDb();
                    Clients = new ObservableCollection<Client>(db.Clients);
                    SignalChanged("Clients");
                    Treatments = new ObservableCollection<Treatment>(db.Treatments);
                    SignalChanged("Treatments");
                    Meals = new ObservableCollection<Meal>(db.Meals);
                    SignalChanged("Meals");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
          
            });


        }

        public void AgeCalculate()
        {
            string a;
            a = Convert.ToString(SelectedClient.Birthday.ToShortDateString());
            var date = DateTime.ParseExact(a, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var age = DateTime.Now.Year - date.Year;
            if (DateTime.Now.Month < date.Month ||
               (DateTime.Now.Month == date.Month && DateTime.Now.Day < date.Day)) age--; ;
            SelectedClientAge = age;
        }


        public ClientAdvancedVM(Client selectedClient) : this()
        {
            SelectedClient = selectedClient;
            SelectedClient.Orders = db.Clients.Where(s => s == selectedClient).SelectMany(b => b.Orders).ToList();
            AgeCalculate();
            
        }
        protected void SignalChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
