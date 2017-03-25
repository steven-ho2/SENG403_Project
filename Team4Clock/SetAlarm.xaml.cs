using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class SetAlarm : UserControl
    {
        private MainWindow mw;            // The parent view object
        private int flag = 0;             // flag is edit alarm or create new alarm

        public SetAlarm(MainWindow newMW, int setFlag)
        {
            this.mw = newMW; // The parent view is set
            this.flag = setFlag;
            this.DataContext = new SetAlarmPresenter(ApplicationService.Instance.EventAggregator);
            InitializeComponent();
        }

        // Removes the current set alarm view from the stack allowing
        // the parent main view to be shown
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
        }


        private void doneBtn_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
            /*
            int hours = Convert.ToInt32(hrLbl.Content) % 12;
            hours = (isPm) ? (hours + 12) : hours;
            hours = (hours == 24) ? 0 : hours;
            TimeSpan alarmTime = new TimeSpan(hours, Convert.ToInt32(min), 0);
            BasicAlarm alarm = new BasicAlarm(alarmTime);

            if (mw.getFlag() == 0)
            {
                mw.setList(alarm);
            }
            else if (mw.getFlag() == 1)
            {
                mw.editChanges(alarm);
            }*/
        }
    }
}
