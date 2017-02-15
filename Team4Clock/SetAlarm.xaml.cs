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
        private bool pmClicked = false;  // checks if the pm button has been pressed
        private bool amClicked = false;  // checks if the am button has been pressed
        private DayOfWeek day = DayOfWeek.Sunday;
        private int amOrPm = 0;         // pm = 1 and am = 2
        private MainWindow mw = new MainWindow(); // The parent view object

        public SetAlarm(MainWindow newMW)
        {
            this.mw = newMW; // The parent view is set 
            InitializeComponent();
            // initialize the Sunday button to "pressed" state (null-day state is not possible)
            sunBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));
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
            amOrPm = 1;
            pmBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));
            pmClicked = true;

            if (amClicked)
            {
                amBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
                amClicked = false;
            }
        }

        private void amBtn_Click(object sender, RoutedEventArgs e)
        {
            amOrPm = 2;
            amBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));
            amClicked = true;

            if (pmClicked)
            {
                pmBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
                pmClicked = false;
            }
        }
        /*
            - Error handling is done is PM or AM and day is missed
            - Alarm object with data is set and passed to the MainWindow class to be used
        */
        private void doneBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((!(amClicked || pmClicked)) && day == 0)
            {
                errLbl.Content = "Please select a day (MON - SUN) and AM or PM";
            }
            else if(!(amClicked || pmClicked))
            {
                errLbl.Content = "Please select AM or PM";
            }
            else
            {
                String min = Convert.ToString(min1Lbl.Content) + Convert.ToString(min2Lbl.Content);
                (this.Parent as Panel).Children.Remove(this);

                int hours = Convert.ToInt32(hrLbl.Content);
                hours = (amOrPm == 1) ? (hours + 12) : hours;
                hours = (hours == 24) ? 0 : hours;
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
        }

        private void sunBtn_Click(object sender, RoutedEventArgs e)
        {
            day = DayOfWeek.Sunday;
            sunBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));
            monBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            tueBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            wedBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            thuBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            friBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            satBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
        }

        private void monBtn_Click(object sender, RoutedEventArgs e)
        {
            day = DayOfWeek.Monday;
            sunBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            monBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));
            tueBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            wedBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            thuBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            friBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            satBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
        }

        private void tueBtn_Click(object sender, RoutedEventArgs e)
        {
            day = DayOfWeek.Tuesday;
            sunBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            monBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            tueBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));
            wedBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            thuBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            friBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            satBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
        }

        private void wedBtn_Click(object sender, RoutedEventArgs e)
        {
            day = DayOfWeek.Wednesday;
            sunBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            monBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            tueBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            wedBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));
            thuBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            friBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            satBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
        }

        private void thuBtn_Click(object sender, RoutedEventArgs e)
        {
            day = DayOfWeek.Thursday;
            sunBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            monBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            tueBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            wedBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            thuBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));
            friBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            satBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
        }

        private void friBtn_Click(object sender, RoutedEventArgs e)
        {
            day = DayOfWeek.Friday;
            sunBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            monBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            tueBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            wedBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            thuBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            friBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));
            satBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
        }

        private void satBtn_Click(object sender, RoutedEventArgs e)
        {
            day = DayOfWeek.Saturday;
            sunBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            monBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            tueBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            wedBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            thuBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            friBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            satBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));
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
