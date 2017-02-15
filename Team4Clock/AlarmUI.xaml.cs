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
    /// <summary>
    /// Interaction logic for Alarm.xaml
    /// </summary>
    public partial class AlarmUI : UserControl
    {

        private Alarm a;

        public AlarmUI(Alarm inputAlarm)
        {
            InitializeComponent();
            //this.a = new Alarm(inputAlarm);
            this.a = inputAlarm;
            alarmTime.Content = a.displayTime();
        }

        public object getAlarm()
        {
            return this.a;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            a.deleteAlarm();
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            a.editAlarm();
        }

        private void onOffBtn_Click(object sender, RoutedEventArgs e)
        {
            // Call the Alarms toggle
            if (a.toggleAlarmOn())
            {
                onOffBtn.Content = "OFF";
            }
            else
            {
                onOffBtn.Content = "ON";
            }
        }
    }
}
