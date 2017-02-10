using System;
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

namespace Team4Clock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SWClock clock;
   
        private List<DateTime> list = new List<DateTime>();
        private int snoozeDelay;
        private int setDelay = 3;

        public MainWindow()
        {
            InitializeComponent();


           

     


            clock = new SWClock();
            
            startClock();
            this.KeyUp += MainWindow_KeyUp;

            activateSnooze();   //Testing snooze function

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

        }

        private void awake_Click(object sender, RoutedEventArgs e)
        {
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
            ListOfAlarms listAlarm = new ListOfAlarms(this, list);
            Main.Children.Add(listAlarm);



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
            Main.Children.Add(setAlarm);
        }

        public void setList(DateTime alarm)
        {
            list.Add(alarm);
            Console.WriteLine("----> List Count: " + list.Count + " Day: " + alarm.Day + " Time: " + 
                                alarm.Hour + ":" + alarm.Minute + " PM(1) AM(2): " + alarm.Second);
        }
        
        // Check whether to activate buttons or keep snoozing
        private void snoozeTick()
        {
            if (snoozeDelay > 0)
            {
                snoozeDelay--;
            }
            else if(snoozeDelay == -1)
            {
                snoozeButton.Visibility = Visibility.Hidden;
                awakeButton.Visibility = Visibility.Hidden;
            }
            else
            {
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
    }
}
