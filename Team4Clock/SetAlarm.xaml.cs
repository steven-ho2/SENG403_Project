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
    public partial class SetAlarm : UserControl
    {
        private DayOfWeek day = DayOfWeek.Sunday;
        private bool isPm = false;        // default to AM
        private MainWindow mw = new MainWindow(); // The parent view object

        // Mappings between buttons and days of week to simplify event handling
        private Dictionary<RadioButton, DayOfWeek> buttonToDay;
        private Dictionary<DayOfWeek, RadioButton> dayToButton;

        public SetAlarm(MainWindow newMW)
        {
            this.mw = newMW; // The parent view is set 
            InitializeComponent();
            initDictionaries();

            DayOfWeek curDay = DateTime.Now.DayOfWeek;
            RadioButton curDayBtn = dayToButton[curDay];
            curDayBtn.IsChecked = true;
            ResolveDayClick(curDayBtn);
        }

        /* Initializes dictionaries used by this class.
         * 
         * This provides fast lookup between days of the week and their corresponding buttons.
         */
        private void initDictionaries()
        {
            // Button->Day mappings
            buttonToDay = new Dictionary<RadioButton, DayOfWeek> {
                {sunBtn, DayOfWeek.Sunday},
                {monBtn, DayOfWeek.Monday},
                {tueBtn, DayOfWeek.Tuesday},
                {wedBtn, DayOfWeek.Wednesday},
                {thuBtn, DayOfWeek.Thursday},
                {friBtn, DayOfWeek.Friday},
                {satBtn, DayOfWeek.Saturday}
            };

            // Day->Button mappings
            dayToButton = new Dictionary<DayOfWeek, RadioButton>();
            foreach (var entry in buttonToDay)
            {
                dayToButton.Add(entry.Value, entry.Key);
            }
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

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            ResolveDayClick((RadioButton)sender);
        }

        private void ResolveDayClick(RadioButton btn)
        {
            day = buttonToDay[btn];
            UpdateDayButtonColours();
        }

        private void UpdateDayButtonColours()
        {
            foreach (var child in DayButtons.Children)
            {
                RadioButton btn = (RadioButton)child;
                if (btn != null)
                {
                    btn.Background = (bool)btn.IsChecked ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070")) :
                                                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
                }
            }
        }

        private List<DayOfWeek> GetCheckboxDays()
        {
            List<DayOfWeek> retList = new List<DayOfWeek>();
            if ((bool)rptBoxSun.IsChecked) retList.Add(DayOfWeek.Sunday);
            if ((bool)rptBoxMon.IsChecked) retList.Add(DayOfWeek.Monday);
            if ((bool)rptBoxTue.IsChecked) retList.Add(DayOfWeek.Tuesday);
            if ((bool)rptBoxWed.IsChecked) retList.Add(DayOfWeek.Wednesday);
            if ((bool)rptBoxThu.IsChecked) retList.Add(DayOfWeek.Thursday);
            if ((bool)rptBoxFri.IsChecked) retList.Add(DayOfWeek.Friday);
            if ((bool)rptBoxSat.IsChecked) retList.Add(DayOfWeek.Saturday);
            return retList;
        }
    }
}