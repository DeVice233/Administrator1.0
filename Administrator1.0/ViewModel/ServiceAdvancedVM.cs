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
    public class ServiceAdvancedVM : INotifyPropertyChanged
    {
        Db db;
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

        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Service> Services { get; set; }

        public CustomCommand ServiceSave { get; set; }

        public ServiceAdvancedVM()
        {
           db = Db.GetDb();
            Clients = new ObservableCollection<Client>(db.Clients);
            SignalChanged("Clients");
            Services = new ObservableCollection<Service>(db.Services);
            SignalChanged("Services");

            ServiceSave = new CustomCommand(o =>
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


        }



        public ServiceAdvancedVM(Service selectedService) : this()
        {
            SelectedService = selectedService;
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
