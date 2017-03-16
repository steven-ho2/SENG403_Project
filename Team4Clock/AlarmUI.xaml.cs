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

namespace Team4Clock
{
    public partial class AlarmUI : UserControl
    {
        public Alarm a {get; private set;}
        public int id;
        private MainWindow mw = new MainWindow();

        public AlarmUI(Alarm inputAlarm, MainWindow mw)
        {
            InitializeComponent();

            DataContext = this;
            this.a = inputAlarm;
            this.mw = mw;
            UpdateLabelColours();
        }

        public object getAlarm()
        {
            return this.a;
        }

        public void setAlarm(Alarm test)
        {
            this.a = test;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            a.deleteAlarm();
            mw.deleteFromListAlarm(this,a);
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            //a.editAlarm();
            mw.editFromListAlarm(this, a);
        }

        private void toggle_Click(object sender, RoutedEventArgs e)
        {
            UpdateLabelColours();
        }

        private void UpdateLabelColours()
        {
            Console.WriteLine("ffffffffffffffffffffffffffffffffffffffff");
            // Call the Alarms toggle
            if (a.toggleAlarmOn())
            {
                onOffBtn.Content = "OFF";
            }
            else
            {
                onOffBtn.Content = "ON";
            }
            Brush color = a.on ? Brushes.White : Brushes.DimGray;
            alarmTime.Foreground = color;
            infoString.Foreground = color;
        }
    }
}
