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
    public class ServiceAdvancedVM : INotifyPropertyChanged
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

            if ((string)SelectedType.Tag == "1")
                if (client.SecondName.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "0")
                if (client.FirstName.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "2")
                if (client.Birthday.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "3")
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
        private Service selectedService;
        public Service SelectedService
        {
            get => selectedService;
            set
            {
                Set(ref selectedService, value);
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
        private Client selectedClient;
        public Client SelectedClient
        {    get => selectedClient;
            set
            {
                Set(ref selectedClient, value);
                SignalChanged();
            }
        }

        public ObservableCollection<Service> Services { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Order> Orders { get; set; }

        public CustomCommand ServiceSave { get; set; }
        public CustomCommand AddClientToOrder { get; set; }
        public CustomCommand AddServiceToOrder { get; set; }

        public ServiceAdvancedVM()
        {
           db = Db.GetDb();
            Clients = new ObservableCollection<Client>(db.Clients);
            SignalChanged("Clients");
            Services = new ObservableCollection<Service>(db.Services);
            SignalChanged("Services"); 
            Orders = new ObservableCollection<Order>(db.Orders);
            SignalChanged("Orders");
            _FilterClients.Source = Clients;
            _FilterClients.Filter += OnClientFiltred;

            ServiceSave = new CustomCommand(o =>
            {
                try
                {
                    db.SaveChanges();
                    Orders = new ObservableCollection<Order>(db.Orders);
                    SignalChanged("Orders");

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }

            });
            AddClientToOrder = new CustomCommand(o =>
            {
                SelectedOrder.Client = SelectedClient;
                Orders = new ObservableCollection<Order>(db.Orders);
                SignalChanged("Orders");
                Clients = new ObservableCollection<Client>(db.Clients);
                SignalChanged("Clients");
                Services = new ObservableCollection<Service>(db.Services);
                SignalChanged("Services");
            });
            AddServiceToOrder = new CustomCommand(o =>
            {
                SelectedOrder.Service = SelectedService;
                Orders = new ObservableCollection<Order>(db.Orders);
                SignalChanged("Orders");
                Clients = new ObservableCollection<Client>(db.Clients);
                SignalChanged("Clients");
                Services = new ObservableCollection<Service>(db.Services);
                SignalChanged("Services");
            });
        }

        public ServiceAdvancedVM(Order selectedOrder) : this()
        {
            SelectedOrder = selectedOrder;
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
