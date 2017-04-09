using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Team4Clock.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlarmList : ContentPage
    {
        /// <summary>
        /// Temporary (?) constructor until AlarmUI controls are created for Xamarin.
        /// 
        /// Takes raw Alarms (rather than AlarmUIs, as planned) to ensure that the list
        /// is able to display the right information (proof-of-concept).
        /// </summary>
        /// <param name="alarms">An ObservableCollection of raw Alarm objects.</param>
        public AlarmList(ObservableCollection<Alarm> alarms)
        {
            Console.WriteLine("Alarms in input set: " + alarms.Count);
            InitializeComponent();
            BindingContext = new AlarmListViewModel(alarms);
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
            => ((ListView)sender).SelectedItem = null;

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            await DisplayAlert("Selected", e.SelectedItem.ToString(), "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }


    /// <summary>
    /// Internal ViewModel class for the AlarmList.
    /// 
    /// This is currently not shared code, because the original app was designed
    /// without MVVM in mind. As a result, AlarmUIs are handled in an unusual way:
    /// the ListView in the MainWindow is maintained by the MainWindow itself.
    /// 
    /// This cannot work in Android/iOS, however, and so a separate View, with
    /// a corresponding ViewModel, is needed.
    /// </summary>
    class AlarmListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Alarm> Alarms { get; set; }

        public ObservableCollection<AlarmUI> AlarmUIs { get; set; }

        public AlarmListViewModel(ObservableCollection<Alarm> alarms)
        {
            Alarms = alarms;

            RefreshDataCommand = new Command(
                async () => await RefreshData());

            AlarmUIs = new ObservableCollection<AlarmUI>();
            foreach (Alarm alarm in Alarms)
            {
                AlarmUIs.Add(new AlarmUI());
            }
        }

        public ICommand RefreshDataCommand { get; }

        async Task RefreshData()
        {
            IsBusy = true;
            //Load Data Here
            await Task.Delay(2000);

            IsBusy = false;
        }

        bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged();
                ((Command)RefreshDataCommand).ChangeCanExecute();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
