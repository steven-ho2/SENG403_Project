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
        ObservableCollection<AlarmUI> collecton;

        public ObservableCollection<AlarmUI> Collection
        {
            get
            {
                return collecton;
            }
            set
            {
                collecton = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Collection"));
                }
            }
        }

        private SoundPlayer _player = new SoundPlayer();
        private string _soundLocation = @"PoliceSound.wav";
        private int flag = 0;
        private AlarmUI editThis;

        public event PropertyChangedEventHandler PropertyChanged;

        private IEventAggregator _eventAggregator;

        public MainWindow()
        {
            Console.WriteLine("Instantiating a MainWindow...");
            this._eventAggregator = ApplicationService.Instance.EventAggregator;
            this.DataContext = new MainPresenter(ApplicationService.Instance.EventAggregator);
            Collection = new ObservableCollection<AlarmUI>();
            InitializeComponent();
            this.KeyUp += MainWindow_KeyUp;

            this._eventAggregator.GetEvent<NewAlarmEvent>().Subscribe((alarm) =>
            {              
                Collection.Add(new AlarmUI(alarm));
            });


            // This should never be null, but better safe than sorry...
            MainPresenter viewModel = this.DataContext as MainPresenter;
            if (viewModel != null)
            {
                viewModel.TriggerAlarm += AlarmEvent;
            }
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
            Console.WriteLine("Alarm Triggered!");

            // old ringing logic
            _player.SoundLocation = _soundLocation;
            _player.Load();
            _player.PlayLooping();
            activateSnooze();
            //alarm.Ring();       // TODO: need to reroute this as command
            
        }

        public object Children { get; internal set; }

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
            setFlag(0);
            SetAlarm setAlarm = new SetAlarm(this, getFlag());
            Main.Children.Add(setAlarm);
        }

        private void rptAlarmBtn_Click(object sender, RoutedEventArgs e)
        {
            SetRepeatAlarm setRptAlarm = new SetRepeatAlarm(this);
            Main.Children.Add(setRptAlarm);
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

        public void deleteFromListAlarm(AlarmUI alarmUI,Alarm alarm)
        {
            collecton.Remove(alarmUI);
            //alarmSet.Remove(alarm);
        }

        private void RefreshAlarmUIs()
        {
            ObservableCollection<AlarmUI> newCollection = new ObservableCollection<AlarmUI>();
            /*foreach (Alarm alarm in alarmSet)
            {
                newCollection.Add(new AlarmUI(alarm, this));
            }*/
            Collection = newCollection;
        }

        public void editFromListAlarm(AlarmUI alarmUI, Alarm alarm)
        {
            setFlag(1);
            editThis = alarmUI;
            SetAlarm setAlarm = new SetAlarm(this, getFlag());
            Main.Children.Add(setAlarm);
        }

        public int getFlag()
        {
            return this.flag;
        }

        public void setFlag(int newFlag)
        {
            this.flag = newFlag;
        }


        // TODO: resolve this
        public void editChanges(Alarm alarm)
        {
            /*AlarmUI alarmUI = new AlarmUI(alarm, this);

            for (int i = 0; i < collecton.Count; i++)
            {
                if (collecton[i] == editThis)
                {
                    collecton[i] = alarmUI;
                }
            }*/
        }


    }
}
