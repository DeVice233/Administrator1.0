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
    public class ServiceVM : INotifyPropertyChanged
    {
        public ComboBoxItem SelectedType { get; set; }
        Db db;
        public Window window { get; set; }
        public ServiceAdvancedVM serviceAdvancedVM = new ServiceAdvancedVM();
        private readonly CollectionViewSource _FilterOrders = new CollectionViewSource();

        public ICollectionView FilterOrders => _FilterOrders?.View;

        private void OnOrderFiltred(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Order order))
            {
                e.Accepted = false;
                return;
            }

            var filter_text = _OrderFilterText;
            if (string.IsNullOrWhiteSpace(filter_text))
                return;
            string status;
            status = StatusConvert(order.Status);
            if (order.Id.ToString() is null || order.Service is null ||
                order.Client is null)
            {
                e.Accepted = false;
                return;
            }

            if ((string)SelectedType.Tag == "0")
                if (order.Id.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "1")
                if (order.Service.ServiceName.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "2")
                if (order.Date.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "3")
                if (order.Client.SecondName.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "4")
                if (status.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            e.Accepted = false;
        }

        private string _OrderFilterText;
        public string OrderFilterText
        {
            get => _OrderFilterText;
            set
            {
                if (SelectedType == null)
                {
                    MessageBox.Show("Выберите тип поиска!");
                    return;
                }
                if (!Set(ref _OrderFilterText, value)) return;
                _FilterOrders.View.Refresh();
            }
        }
        private Order selectedOrder = new Order { };
        public Order SelectedOrder
        {
            get => selectedOrder;
            set
            {
                Set(ref selectedOrder, value);
                SignalChanged();
            }
        }
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Service> Services { get; set; }

        public CustomCommand OpenServiceAdvanced { get; set; }
        public CustomCommand OpenNewServiceAdvanced { get; set; }

        public ServiceVM()
        {
            db = Db.GetDb();
            Clients = new ObservableCollection<Client>(db.Clients);
            SignalChanged("Clients");
            Services = new ObservableCollection<Service>(db.Services);
            SignalChanged("Services");
            Orders = new ObservableCollection<Order>(db.Orders);
            SignalChanged("Orders");
            _FilterOrders.Source = Orders;
            _FilterOrders.Filter += OnOrderFiltred;

            OpenServiceAdvanced = new CustomCommand(o =>
            {
                window = new ServiceAdvanced(SelectedOrder);
                window.ShowDialog();
            });
            OpenNewServiceAdvanced = new CustomCommand(o =>
            {

                var order = new Order { };
                db.Orders.Add(order);
                SelectedOrder = order;
                Orders = new ObservableCollection<Order>(db.Orders);
                SignalChanged("Orders");
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
                window = new ServiceAdvanced(SelectedOrder);
                window.ShowDialog();
            });
        }
        public string StatusConvert(string status)
        {

            if (status == "done")
            {
                status = "Оплачен";
            }
            if (status == "canceled")
            {
                status = "Отменен";
            }
            if (status == "confirmed")
            {
                status = "Подтверждено";
            }
            if (status == null)
            {
                status = "Не указан";
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
