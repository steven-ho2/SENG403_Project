using System;
using System.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.ComponentModel;
using Prism.Events;

namespace Team4Clock
{
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        private ObservableCollection<AlarmUI> collection;
        private Dictionary<Alarm, AlarmUI> _alarmUIMap = new Dictionary<Alarm,AlarmUI>();

        public ObservableCollection<AlarmUI> Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Collection"));
                }
            }
        }

        private SoundPlayer _player = new SoundPlayer();
        private string _soundLocation = @"PoliceSound.wav";

        public event PropertyChangedEventHandler PropertyChanged;

        private IEventAggregator _eventAggregator;

        public MainWindow()
        {
            this._eventAggregator = ApplicationService.Instance.EventAggregator;
            this.DataContext = new MainPresenter(ApplicationService.Instance.EventAggregator);
            Collection = new ObservableCollection<AlarmUI>();
            InitializeComponent();
            this.KeyUp += MainWindow_KeyUp;
            SubscribeToEvents();

            // This should never be null, but better safe than sorry...
            MainPresenter viewModel = this.DataContext as MainPresenter;
            if (viewModel != null)
            {
                viewModel.TriggerAlarm += AlarmEvent;
            }
        }

        private void SubscribeToEvents()
        {
            this._eventAggregator.GetEvent<NewAlarmEvent>().Subscribe((alarm) =>
            {
                AddAlarm(alarm);
            });

            this._eventAggregator.GetEvent<DeleteAlarmEvent>().Subscribe((alarm) =>
            {
                DeleteAlarm(alarm);
            });


            this._eventAggregator.GetEvent<EditAlarmEvent>().Subscribe((wrapper) =>
            {
                DeleteAlarm(wrapper.OldAlarm);
                AddAlarm(wrapper.NewAlarm);
            });

            this._eventAggregator.GetEvent<RequestEditAlarmEvent>().Subscribe((alarm) =>
            {
                OpenEditAlarm(alarm);
            });
        }

        private void AddAlarm(Alarm alarm)
        {
            AlarmUI alarmUI = new AlarmUI(alarm);
            _alarmUIMap.Add(alarm, alarmUI);
            Collection.Add(alarmUI);
        }

        private void DeleteAlarm(Alarm alarm)
        {
            AlarmUI delAlarmUI = _alarmUIMap[alarm];
            Collection.Remove(delAlarmUI);
            _alarmUIMap.Remove(alarm);
        }

        private void OpenEditAlarm(Alarm alarm)
        {
            Type alarmType = alarm.GetType();
            if (alarmType == typeof(BasicAlarm))
                SetAlarmView(alarm as BasicAlarm);
            else
                SetRepeatView(alarm as RepeatingAlarm);
        }

        private void SetAlarmView(BasicAlarm alarm = null)
        {
            SetAlarm setAlarm;
            if (alarm == null)
                // set new alarm
                setAlarm = new SetAlarm();
            else
                // edit alarm
                setAlarm = new SetAlarm(alarm);
            Main.Children.Add(setAlarm);
        }

        private void SetRepeatView(RepeatingAlarm alarm = null)
        {
            SetRepeatAlarm setAlarm;
            if (alarm == null)
                // set new alarm
                setAlarm = new SetRepeatAlarm();
            else
                //edit alarm
                setAlarm = new SetRepeatAlarm(alarm);
            Main.Children.Add(setAlarm);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void AlarmEvent(object sender, EventArgs e)
        {
            _player.SoundLocation = _soundLocation;
            _player.Load();
            _player.PlayLooping();
            activateSnooze();            
        }

        private void awake_Click(object sender, RoutedEventArgs e)
        {
            _player.Stop();
            
            awakeButton.Visibility = Visibility.Hidden;
            snoozeButton.Visibility = Visibility.Hidden;

            //RefreshAlarmUIs();
        }

        // Activate snooze and wake up buttons, set snooze delay
        private void snooze_Click(object sender, RoutedEventArgs e)
        {
           snoozeButton.Visibility = Visibility.Hidden;
           awakeButton.Visibility = Visibility.Hidden;
           //snoozeDelay = setDelay;      // TODO: need to reroute this as command
        }
        
        //Activate the snooze buttons
        public void activateSnooze()
        {
            snoozeButton.Visibility = Visibility.Visible;
            awakeButton.Visibility = Visibility.Visible;
        }

        private void setAlarmBtn_Click(object sender, RoutedEventArgs e)
        {
            SetAlarmView();
        }

        private void rptAlarmBtn_Click(object sender, RoutedEventArgs e)
        {
            SetRepeatView();
        }

        public void setList(Alarm alarm)
        {
            //list.Add(alarm, false);
            //AlarmUI alarmUI = new AlarmUI(alarm, this);
            //alarmSet.Add(alarm);
            //collecton.Add(alarmUI);
        }

        private void toggleBtn_Click(object sender, RoutedEventArgs e)
        {
            Analog analog = new Analog(this);
            Main.Children.Add(analog);
        }
    }
}
