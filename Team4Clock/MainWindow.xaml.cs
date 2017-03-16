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

namespace Team4Clock
{
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        private SWClock clock;
        private List<Alarm> alarmSet = new List<Alarm>();
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
        private int snoozeDelay;
        private int setDelay = 5;
        private bool alarmOn = false;
        private SoundPlayer player = new SoundPlayer();
        string path = Assembly.GetExecutingAssembly().Location;
        private String soundLocation = @"PoliceSound.wav";
        private bool played = false;
        private int flag = 0;
        private AlarmUI editThis;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            Collection = new ObservableCollection<AlarmUI>();
            InitializeComponent();
            clock = new SWClock();
            startClock();
            this.KeyUp += MainWindow_KeyUp;
            collecton.CollectionChanged += HandleChange;
            snoozeDelay = -2;
        
            //activateSnooze();   //Testing snooze function

        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // do something  
            }
           
            else if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                
                // do something
            }
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        //This is the main event handler for displaying the time
        private void startClock()
        {
            this.TImeLabel.Content = clock.ShowTime; //display the inital time to label
            DispatcherTimer time = new DispatcherTimer(); //This is the timer to a handle the ticking
            time.Tick += new EventHandler(time_tick);
            time.Interval = new TimeSpan(0, 0, 1); //Set the wait time to 1 min //I changed it to 1 sec to check snooze
            time.Start();
        }

        public Grid getGrid
        {
            get { return Main; }
        }

        public object Children { get; internal set; }

        //Update the label with the current time
        private void time_tick(object sender, EventArgs e)
        {
            this.TImeLabel.Content = clock.ShowTime;
            snoozeTick();
            foreach (Alarm alarm in alarmSet)
            {
                if (DateTime.Compare(clock.getCurrentTime(), alarm.time) == 0)
                {
                    if (alarm.on && alarmOn == false && played == false)
                    {
                        played = true;
                        this.player.SoundLocation = soundLocation;
                        player.Load();
                        this.player.PlayLooping();
                        alarmOn = true;
                        activateSnooze();
                        alarm.Ring();
                    }
                }
            }
        }

        private void awake_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            played = false;
            snoozeDelay = -2;
            
            awakeButton.Visibility = Visibility.Hidden;
            snoozeButton.Visibility = Visibility.Hidden;

            Console.WriteLine(alarmSet.Count);

            foreach (Alarm alarm in alarmSet)
            {
                if (alarm.ringing) 
                    alarm.WakeUp();
            }

            RefreshAlarmUIs();
        }

        // Activate snooze and wake up buttons, set snooze delay
        private void snooze_Click(object sender, RoutedEventArgs e)
        {
           snoozeButton.Visibility = Visibility.Hidden;
           awakeButton.Visibility = Visibility.Hidden;
           snoozeDelay = setDelay;
        }

        //Event for when "list of alarm" button is clicked
        private void List_Click(object sender, RoutedEventArgs e)
        {
            // dummy
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
            SetAlarm setAlarm = new SetAlarm(this,0);
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
            AlarmUI alarmUI = new AlarmUI(alarm, this);
            alarmSet.Add(alarm);
            collecton.Add(alarmUI);
        }

        private void toggleBtn_Click(object sender, RoutedEventArgs e)
        {
            Analog analog = new Analog(this);
            Main.Children.Add(analog);
        }

        // Check whether to activate buttons or keep snoozing
        private void snoozeTick()
        {
            if (snoozeDelay >= 0)
            {
                if (alarmOn == true)
                {
                    player.Stop();
                    alarmOn = false;
                    snoozeButton.Visibility = Visibility.Hidden;
                    awakeButton.Visibility = Visibility.Hidden;
                }
                snoozeDelay--;
            }
            else if (snoozeDelay == -1)
            {
                player.SoundLocation = soundLocation;
                player.Load();
                this.player.PlayLooping();
                alarmOn = true;
                snoozeDelay = -2;
                activateSnooze();
            }
            
        }
        // Set snooze delay
        public void setSnoozeDelay(int delay)
        {
            setDelay = delay;
        }
        // Get snooze delay
        public int getSnoozeDelay()
        {
            return snoozeDelay;
        }
        public void deleteFromListAlarm(AlarmUI alarmUI,Alarm alarm)
        {
            collecton.Remove(alarmUI);
            alarmSet.Remove(alarm);
        }

        private void RefreshAlarmUIs()
        {
            ObservableCollection<AlarmUI> newCollection = new ObservableCollection<AlarmUI>();
            foreach (Alarm alarm in alarmSet)
            {
                newCollection.Add(new AlarmUI(alarm, this));
            }
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


        public void editChanges(Alarm alarm)
        {
            AlarmUI alarmUI = new AlarmUI(alarm, this);

            for(int i = 0; i < collecton.Count; i++)
            {
                if(collecton[i] == editThis)
                {
                    collecton[i] = alarmUI;
                }
            }
        }
    }
}
