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
        private bool isPm = false;        // default to AM
        private MainWindow mw = new MainWindow(); // The parent view object
        private int flag = 0; // flag is edit alarm or create new alarm
        // Mappings between buttons and days of week to simplify event handling
        private Dictionary<CheckBox, DayOfWeek> buttonToDay;
        private Dictionary<DayOfWeek, CheckBox> dayToButton;

        // Collections of strings for use in ComboBoxes for alarm repeats. Set by initComboBoxes() function.
        private ObservableCollection<string> hrsList = new ObservableCollection<string>();
        private ObservableCollection<string> minsList = new ObservableCollection<string>();
        private ObservableCollection<string> amPmList = new ObservableCollection<string>();


        public SetAlarm(MainWindow newMW, int setFlag)
        {
            this.mw = newMW; // The parent view is set 
            this.flag = setFlag;
            InitializeComponent();
        }

        // Removes the current set alarm view from the stack allowing
        // the parent main view to be shown
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
        }

        private void hrBtnUp_Click(object sender, RoutedEventArgs e)
        {
            int hr = Convert.ToInt32(hrLbl.Content);

            if (hr < 12)
            {
                hrLbl.Content = hr + 1;
            }
            else
            {
                hrLbl.Content = 1;
            }
        }

        private void hrDownBtn_Click(object sender, RoutedEventArgs e)
        {
            int hr = Convert.ToInt32(hrLbl.Content);

            if (hr > 1)
            {
                hrLbl.Content = hr - 1;
            }
            else
            {
                hrLbl.Content = 12;
            }
        }

        private void min1BtnUp_Click(object sender, RoutedEventArgs e)
        {
            int min = Convert.ToInt32(min1Lbl.Content);

            if (min < 5)
            {
                min1Lbl.Content = min + 1;
            }
            else
            {
                min1Lbl.Content = 0;
            }
        }

        private void min1DownBtn_Click(object sender, RoutedEventArgs e)
        {
            int min = Convert.ToInt32(min1Lbl.Content);

            if (min > 0)
            {
                min1Lbl.Content = min - 1;
            }
            else
            {
                min1Lbl.Content = 5;
            }
        }

        private void min2BtnUp_Click(object sender, RoutedEventArgs e)
        {
            int min = Convert.ToInt32(min2Lbl.Content);

            if (min < 9)
            {
                min2Lbl.Content = min + 1;
            }
            else
            {
                min2Lbl.Content = 0;
            }
        }

        private void min2BtnDown_Click(object sender, RoutedEventArgs e)
        {
            int min = Convert.ToInt32(min2Lbl.Content);

            if (min > 0)
            {
                min2Lbl.Content = min - 1;
            }
            else
            {
                min2Lbl.Content = 9;
            }
        }

        private void pmBtn_Click(object sender, RoutedEventArgs e)
        {
            isPm = true;
        }

        private void amBtn_Click(object sender, RoutedEventArgs e)
        {
            isPm = false;
        }
        /*
            - Error handling is currently not done as there is no need (it should be impossible to not set AM/PM or day of week)
            - Alarm object with data is set and passed to the MainWindow class to be used
        */
        private void doneBtn_Click(object sender, RoutedEventArgs e)
        {
            String min = Convert.ToString(min1Lbl.Content) + Convert.ToString(min2Lbl.Content);
            (this.Parent as Panel).Children.Remove(this);

            int hours = Convert.ToInt32(hrLbl.Content) % 12;
            hours = (isPm) ? (hours + 12) : hours;
            hours = (hours == 24) ? 0 : hours;
            TimeSpan alarmTime = new TimeSpan(hours, Convert.ToInt32(min), 0);
            BasicAlarm alarm = new BasicAlarm(alarmTime);

            if (mw.getFlag() == 0) {
                mw.setList(alarm);
            }
            else if (mw.getFlag() == 1)
            {
                mw.editChanges(alarm);
            }
 
        }

        private List<DayOfWeek> GetCheckboxDays()
        {
            List<DayOfWeek> retList = new List<DayOfWeek>();
            return retList;
        }

        private TimeSpan parseRepeatBoxes(ComboBox hoursBox, ComboBox minsBox, ComboBox amPm)
        {
            bool setPm = amPm.SelectedItem.ToString() == "PM";
            string hoursStr = hoursBox.SelectedItem.ToString();
            int hours = (hoursStr == "12") ? 0 : Int32.Parse(hoursBox.SelectedItem.ToString());
            hours += setPm ? 12 : 0;
            int mins = Int32.Parse(minsBox.SelectedItem.ToString());
            return new TimeSpan(hours, mins, 0);
        }
    }
}
