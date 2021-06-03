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

namespace Administrator1._0.ViewModel
{
    public class ServiceVM : INotifyPropertyChanged
    {
        Db db;
        public Window window { get; set; }
        public ServiceAdvancedVM serviceAdvancedVM = new ServiceAdvancedVM();

        private Service selectedService = new Service { };
        public Service SelectedService
        {
            get => selectedService;
            set
            {
                Set(ref selectedService, value);
                SignalChanged();
            }
        }
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
            OpenServiceAdvanced = new CustomCommand(o =>
            {
                window = new ServiceAdvanced(SelectedService);
                window.ShowDialog();
            });
            OpenNewServiceAdvanced = new CustomCommand(o =>
            {

                var service = new Service { };
                db.Services.Add(service);
                SelectedService = service;
                Services = new ObservableCollection<Service>(db.Services);
                SignalChanged("Services");
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
                window = new ServiceAdvanced(SelectedService);
                window.ShowDialog();
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
