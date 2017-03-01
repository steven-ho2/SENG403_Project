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
        private DayOfWeek day = DayOfWeek.Sunday;
        private bool isPm = false;        // default to AM
        private MainWindow mw = new MainWindow(); // The parent view object

        // Mappings between buttons and days of week to simplify event handling
        private Dictionary<CheckBox, DayOfWeek> buttonToDay;
        private Dictionary<DayOfWeek, CheckBox> dayToButton;

        // Collections of strings for use in ComboBoxes for alarm repeats. Set by initComboBoxes() function.
        private ObservableCollection<string> hrsList = new ObservableCollection<string>();
        private ObservableCollection<string> minsList = new ObservableCollection<string>();
        private ObservableCollection<string> amPmList = new ObservableCollection<string>();


        public SetAlarm(MainWindow newMW)
        {
            this.mw = newMW; // The parent view is set 
            InitializeComponent();
            initDictionaries();

            initComboBoxes();
        }

        /* Initializes dictionaries used by this class.
         * 
         * This provides fast lookup between days of the week and their corresponding buttons.
         */
        private void initDictionaries()
        {
            // Button->Day mappings
            buttonToDay = new Dictionary<CheckBox, DayOfWeek> {
                {sunBtn, DayOfWeek.Sunday},
                {monBtn, DayOfWeek.Monday},
                {tueBtn, DayOfWeek.Tuesday},
                {wedBtn, DayOfWeek.Wednesday},
                {thuBtn, DayOfWeek.Thursday},
                {friBtn, DayOfWeek.Friday},
                {satBtn, DayOfWeek.Saturday}
            };

            // Day->Button mappings
            dayToButton = new Dictionary<DayOfWeek, CheckBox>();
            foreach (var entry in buttonToDay)
            {
                dayToButton.Add(entry.Value, entry.Key);
            }
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

            int hours = Convert.ToInt32(hrLbl.Content);

            hours = (isPm) ? ((hours == 12) ? hours : hours + 12) : (hours == 12) ? 0: hours;

            //if(isPm == true)
            //{
            //    if(hours == 12)
            //    {
            //        hours = hours + 24;
            //    }
            //    else
            //    {
            //        hours = hours + 12;
            //    }
            //    Console.WriteLine("Inside PM");
            //}
            //else
            //{
            //    if (hours == 12)
            //    {
            //        hours = hours + 12;
            //    }
            //}
            //hours = (!isPm) ? (hours + 12) : hours;
            //hours = (hours == 24) ? 0 : hours;
            TimeSpan alarmTime = new TimeSpan(hours, Convert.ToInt32(min), 0);
            Alarm alarm = new Alarm(alarmTime);

            // Get repeat days and update the alarm with these days
            // Todo: update for variable repeats (i.e. different times for different days)
            List<DayOfWeek> rptDays = GetCheckboxDays();
            foreach (DayOfWeek rptDay in rptDays)
            {
                alarm.SetRepeat(rptDay, true);
            }

            mw.setList(alarm);
        }

        private List<DayOfWeek> GetCheckboxDays()
        {
            List<DayOfWeek> retList = new List<DayOfWeek>();
            return retList;
        }
    }
}