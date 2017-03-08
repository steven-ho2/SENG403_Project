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
    public partial class SetRepeatAlarm : UserControl
    {
        private DayOfWeek day = DayOfWeek.Sunday;
        private bool isPm = false;        // default to AM
        private MainWindow mw = new MainWindow(); // The parent view object

        // Collections of strings for use in ComboBoxes for alarm repeats. Set by initComboBoxes() function.
        private ObservableCollection<string> hrsList = new ObservableCollection<string>();
        private ObservableCollection<string> minsList = new ObservableCollection<string>();
        private ObservableCollection<string> amPmList = new ObservableCollection<string>();


        public SetRepeatAlarm(MainWindow newMW)
        {
            this.mw = newMW; // The parent view is set 
            InitializeComponent();

            initComboBoxes();
        }

        private void initComboBoxes()
        {
            // Set up shared string collections for combo boxes
            for (int i = 1; i <= 12; i++)
            {
                hrsList.Add(i.ToString());
            }
            for (int i = 0; i < 60; i++)
            {
                minsList.Add(i.ToString("D2"));
            }
            amPmList.Add("AM");
            amPmList.Add("PM");

            // Set combo boxes to use shared sources (there's gotta be a better way to do this...)
            this.sunHrs.ItemsSource = hrsList;
            this.sunMins.ItemsSource = minsList;
            this.sunAmPm.ItemsSource = amPmList;
            this.monHrs.ItemsSource = hrsList;
            this.monMins.ItemsSource = minsList;
            this.monAmPm.ItemsSource = amPmList;
            this.tueHrs.ItemsSource = hrsList;
            this.tueMins.ItemsSource = minsList;
            this.tueAmPm.ItemsSource = amPmList;
            this.wedHrs.ItemsSource = hrsList;
            this.wedMins.ItemsSource = minsList;
            this.wedAmPm.ItemsSource = amPmList;
            this.thuHrs.ItemsSource = hrsList;
            this.thuMins.ItemsSource = minsList;
            this.thuAmPm.ItemsSource = amPmList;
            this.friHrs.ItemsSource = hrsList;
            this.friMins.ItemsSource = minsList;
            this.friAmPm.ItemsSource = amPmList;
            this.satHrs.ItemsSource = hrsList;
            this.satMins.ItemsSource = minsList;
            this.satAmPm.ItemsSource = amPmList;

            // Set up defaults
            sunHrs.SelectedItem = "12";
            sunMins.SelectedIndex = 0;
            sunAmPm.SelectedIndex = 0;

            monHrs.SelectedItem = "12";
            monMins.SelectedIndex = 0;
            monAmPm.SelectedIndex = 0;

            tueHrs.SelectedItem = "12";
            tueMins.SelectedIndex = 0;
            tueAmPm.SelectedIndex = 0;

            wedHrs.SelectedItem = "12";
            wedMins.SelectedIndex = 0;
            wedAmPm.SelectedIndex = 0;

            thuHrs.SelectedItem = "12";
            thuMins.SelectedIndex = 0;
            thuAmPm.SelectedIndex = 0;

            friHrs.SelectedItem = "12";
            friMins.SelectedIndex = 0;
            friAmPm.SelectedIndex = 0;

            satHrs.SelectedItem = "12";
            satMins.SelectedIndex = 0;
            satAmPm.SelectedIndex = 0;
        }

        // Removes the current set alarm view from the stack allowing
        // the parent main view to be shown
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
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

            // Get repeat days and update the alarm with these days
            RepeatingAlarm alarm = new RepeatingAlarm();
            setAlarmRepeats(alarm);

            if (alarm.HasRepeats())
            {
                (this.Parent as Panel).Children.Remove(this);
                mw.setList(alarm);
            }
            else
            {
                errLabel.Visibility = Visibility.Visible;
            }
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

        private void setAlarmRepeats(RepeatingAlarm alarm)
        {
            TimeSpan time;
            if ((bool) sunBtn.IsChecked) {
                time = parseRepeatBoxes(sunHrs, sunMins, sunAmPm);
                alarm.SetRepeat(DayOfWeek.Sunday, /*repeats*/ true, time);
            }
            if ((bool)monBtn.IsChecked)
            {
                time = parseRepeatBoxes(monHrs, monMins, monAmPm);
                alarm.SetRepeat(DayOfWeek.Monday, /*repeats*/ true, time);
            }
            if ((bool)tueBtn.IsChecked)
            {
                time = parseRepeatBoxes(tueHrs, tueMins, tueAmPm);
                alarm.SetRepeat(DayOfWeek.Tuesday, /*repeats*/ true, time);
            }
            if ((bool)wedBtn.IsChecked)
            {
                time = parseRepeatBoxes(wedHrs, wedMins, wedAmPm);
                alarm.SetRepeat(DayOfWeek.Wednesday, /*repeats*/ true, time);
            }
            if ((bool)thuBtn.IsChecked)
            {
                time = parseRepeatBoxes(thuHrs, thuMins, thuAmPm);
                alarm.SetRepeat(DayOfWeek.Thursday, /*repeats*/ true, time);
            }
            if ((bool)friBtn.IsChecked)
            {
                time = parseRepeatBoxes(friHrs, friMins, friAmPm);
                alarm.SetRepeat(DayOfWeek.Friday, /*repeats*/ true, time);
            }
            if ((bool)satBtn.IsChecked)
            {
                time = parseRepeatBoxes(satHrs, satMins, satAmPm);
                alarm.SetRepeat(DayOfWeek.Saturday, /*repeats*/ true, time);
            }
        }
    }
}
