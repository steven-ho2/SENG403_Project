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
using Prism.Events;

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

        protected override void OnDisappearing()
        {
            (BindingContext as AlarmListViewModel).UnsubscribeAll();
        }

        protected override void OnAppearing()
        {
            (BindingContext as AlarmListViewModel).SubscribeAll();
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
        private ObservableCollection<Alarm> _alarms { get; set; }

        public ObservableCollection<AlarmUIPresenter> AlarmUIViewModels { get; set; }

        private Dictionary<Alarm, AlarmUIPresenter> _alarmMap = new Dictionary<Alarm, AlarmUIPresenter>();
        private IEventAggregator _eventAggregator;
        private bool _subscribed = false;

        public AlarmListViewModel(ObservableCollection<Alarm> alarms)
        {
            _eventAggregator = ApplicationService.Instance.EventAggregator;
            _alarms = alarms;

            RefreshDataCommand = new Command(
                async () => await RefreshData());

            AlarmUIViewModels = new ObservableCollection<AlarmUIPresenter>();
            foreach (Alarm alarm in _alarms)
            {
                AlarmUIPresenter presenter = new AlarmUIPresenter(alarm, ApplicationService.Instance.EventAggregator);
                AlarmUIViewModels.Add(presenter);
                _alarmMap.Add(alarm, presenter);
            }

            SubscribeAll();
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

        public void UnsubscribeAll()
        {
            if (_subscribed)
            {
                _eventAggregator.GetEvent<DeleteAlarmEvent>().Unsubscribe(AlarmRemovedEvent);
                _subscribed = false;
            }
        }

        public void SubscribeAll()
        {
            if (!_subscribed)
            {
                _eventAggregator.GetEvent<DeleteAlarmEvent>().Subscribe(AlarmRemovedEvent);
                _subscribed = true;
            }
        }

        void AlarmRemovedEvent(Alarm alarm)
        {
            Console.WriteLine("Received DeleteAlarmEvent for alarm time: " + alarm.time);
            Console.WriteLine("Dictionary length = " + _alarmMap.Count);

            AlarmUIViewModels.Remove(_alarmMap[alarm]);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
