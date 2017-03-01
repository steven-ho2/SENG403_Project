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

namespace Team4Clock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private SWClock clock;
        private List<Alarm> list = new List<Alarm>();
        private ObservableCollection<AlarmUI> collecton = new ObservableCollection<AlarmUI>();
        private int snoozeDelay;
        private int setDelay = 3;
        private bool alarmOn = false;
        private SoundPlayer player = new SoundPlayer();
        string path = Assembly.GetExecutingAssembly().Location;
        private String soundLocation = @"PoliceSound.wav";
        bool playing = false;

        public MainWindow()
        {
            InitializeComponent();
            clock = new SWClock();
            startClock();
            this.KeyUp += MainWindow_KeyUp;
            collecton.CollectionChanged += HandleChange;
            listTemp.ItemsSource = collecton;
            snoozeDelay = -1;
        
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

        //Update the label with the current time
        private void time_tick(object sender, EventArgs e)
        {
            this.TImeLabel.Content = clock.ShowTime;
            snoozeTick();
            foreach (Alarm alarm in list)
            {
                if(DateTime.Compare(clock.getCurrentTime(), alarm.time) == 0)
                {
                    if (alarmOn == false)
                    { 
                        this.player.SoundLocation = soundLocation;
                        this.player.PlayLooping();
                        Console.WriteLine("Time" + alarm.time);
                        alarmOn = true;
                        snoozeDelay = -2;
                        activateSnooze();
                    }
                }
            }
        }

        private void awake_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();

            awakeButton.Visibility = Visibility.Hidden;
            snoozeButton.Visibility = Visibility.Hidden;

            snoozeDelay = -1;

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
            // ListOfAlarms listAlarm = new ListOfAlarms(this, list);
            // Main.Children.Add(listAlarm);
           
        }
        
        //Activate the snooze buttons
        public void activateSnooze()
        {
            snoozeButton.Visibility = Visibility.Visible;
            awakeButton.Visibility = Visibility.Visible;
        }

        private void setAlarmBtn_Click(object sender, RoutedEventArgs e)
        {
            SetAlarm setAlarm = new SetAlarm(this);
            setAlarm.RenderTransform = new ScaleTransform(2.8, 2.5);
            Main.Children.Add(setAlarm);
        }

        public void setList(Alarm alarm)
        {
            list.Add(alarm);
            AlarmUI alarmUI = new AlarmUI(alarm, this);
            collecton.Add(alarmUI);
        }
        
        // Check whether to activate buttons or keep snoozing
        private void snoozeTick()
        {
            if (snoozeDelay > 0)
            {
                if (alarmOn == true)
                {
                    player.Stop();
                    alarmOn = false;
                }
                snoozeDelay--;
            }
            else if(snoozeDelay == -1)
            {
               

                snoozeButton.Visibility = Visibility.Hidden;
                awakeButton.Visibility = Visibility.Hidden;
            }
            else
            {
                player.SoundLocation = soundLocation;
                this.player.PlayLooping();
                alarmOn = true;
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
            list.Remove(alarm);
            //this.listStack.Children.RemoveAt(id);
        }
    }
}
