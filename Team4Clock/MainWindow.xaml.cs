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

namespace Team4Clock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SWClock clock;

        public MainWindow()
        {
            InitializeComponent();
            clock = new SWClock();
            startClock();
        }

        //This is the main event handler for displaying the time
        private void startClock()
        {
            this.TImeLabel.Content = clock.ShowTime; //display the inital time to label

            DispatcherTimer time = new DispatcherTimer(); //This is the timer to a handle the ticking
            time.Tick += new EventHandler(time_tick);
            time.Interval = new TimeSpan(0, 1, 0); //Set the wait time to 1 min
            time.Start();

        }

        //Update the label with the current time
        private void time_tick(object sender, EventArgs e)
        {
            this.TImeLabel.Content = clock.ShowTime;
        }

        private void setAlarmBtn_Click(object sender, RoutedEventArgs e)
        {
            SetAlarm setAlarm = new SetAlarm(this);
            Main.Children.Add(setAlarm);
        }
    }
}
