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
    public class ClientVM : INotifyPropertyChanged
    {
        public ComboBoxItem SelectedType { get; set; }

        public Page CurrentPage { get; set; }
        public Window window { get; set; }
        public ClientAdvanced clientAdvanced = new ClientAdvanced();
        Db db;

        private readonly CollectionViewSource _FilterClients = new CollectionViewSource();
        
        public ICollectionView FilterClients => _FilterClients?.View;

        private void OnClientFiltred(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Client client))
            { e.Accepted = false; return;}
            var filter_text = _ClientFilterText;
            if (string.IsNullOrWhiteSpace(filter_text)) return;
            if (client.FirstName is null || client.SecondName is null || client.FatherName is null)
            { e.Accepted = false; return; }
            if ((string)SelectedType.Tag == "0")
                if (client.FirstName.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "1")
                if (client.SecondName.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "2")
                if (client.FatherName.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "3")
                if (client.Birthday.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "4")
                if (client.PassportNumber.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "5")
                if (client.PassportSerias.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if ((string)SelectedType.Tag == "6")
                if (client.Tour.Id.ToString().Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            e.Accepted = false;
        }

        private Client selectedClient = new Client {};
        public Client SelectedClient
        {
            get => selectedClient;
            set
            {
                Set(ref selectedClient, value);
                SignalChanged();
            }
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
                //Search();
            }
        }

        public ObservableCollection<Client> Clients { get;set;}
        public ObservableCollection<Treatment> Treatments { get; set; }
        public ObservableCollection<Meal> Meals { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }

        public CustomCommand OpenClientAdvanced { get; set; }
        public CustomCommand OpenNewClientAdvanced { get; set; }
        public CustomCommand DeleteClient { get; set; }

        public ClientVM()
        {
            db = Db.GetDb();
            Clients = new ObservableCollection<Client>(db.Clients);
            SignalChanged("Clients");
            Tours = new ObservableCollection<Tour>(db.Tours);
            SignalChanged("Tours");
            Treatments = new ObservableCollection<Treatment>(db.Treatments);
            SignalChanged("Treatments");
            Meals = new ObservableCollection<Meal>(db.Meals);
            SignalChanged("Meals");

            _FilterClients.Source = Clients;
            _FilterClients.Filter += OnClientFiltred;

            OpenClientAdvanced = new CustomCommand(o =>
             {
               window = new ClientAdvanced(SelectedClient);
                window.ShowDialog();
             });
            DeleteClient = new CustomCommand(o =>
            {
                if (SelectedClient != null)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Удалить выбранного клиента?", "Предупреждение",
                         MessageBoxButton.OKCancel, MessageBoxImage.Warning
                        );
                    if (messageBoxResult == MessageBoxResult.OK)
                    {
                        try
                        {
                            db.Clients.Remove(SelectedClient);
                            db.SaveChanges();
                            Clients = new ObservableCollection<Client>(db.Clients);
                            SignalChanged("Clients");
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                    }
                }
            });
            OpenNewClientAdvanced = new CustomCommand(o =>
            {
                //MessageBoxResult Result = MessageBox.Show(
                //       "Создать нового клиента?",
                //       "Сообщение",
                //       MessageBoxButton.OKCancel,
                //       MessageBoxImage.Question
                //       );
                //if (Result == MessageBoxResult.Yes)
                //{
                    var client = new Client { Sex = "М" };
                    db.Clients.Add(client);
                    SelectedClient = client;
                    Clients = new ObservableCollection<Client>(db.Clients);
                    SignalChanged("Clients");
                try
                {
                    db.SaveChanges();
                    Clients = new ObservableCollection<Client>(db.Clients);
                    SignalChanged("Clients");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }


                window = new ClientAdvanced(SelectedClient);
                    window.ShowDialog();
                //}
            });


        }

        //private void Search()
        //{
        //    //SearchTypes id = (SearchTypes)int.Parse((string)SelectedType.Tag);
        //    if ((string)SelectedType.Tag == "0")
        //        SelectedClient = Clients.FirstOrDefault(s => s.FirstName.StartsWith(Pattern));
        //    else if ((string)SelectedType.Tag == "1")
        //        SelectedClient = Clients.FirstOrDefault(s => s.SecondName.StartsWith(Pattern));
        //    else if ((string)SelectedType.Tag == "2")
        //        SelectedClient = Clients.FirstOrDefault(s => s.FatherName.StartsWith(Pattern));
        //    else if ((string)SelectedType.Tag == "3")
        //        SelectedClient = Clients.FirstOrDefault(s => s.Birthday.ToString().StartsWith(Pattern));
        //    else if ((string)SelectedType.Tag == "4")
        //        SelectedClient = Clients.FirstOrDefault(s => s.PassportNumber.ToString().StartsWith(Pattern));
        //    else if ((string)SelectedType.Tag == "5")
        //        SelectedClient = Clients.FirstOrDefault(s => s.PassportSerias.ToString().StartsWith(Pattern));
        //}


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
