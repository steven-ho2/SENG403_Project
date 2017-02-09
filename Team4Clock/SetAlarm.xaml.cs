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
        private int day = 0;            // SUN = 0 MON = 1 TUE = 2 WED = 3 THU = 4 FRI = 5 SAT = 6
        private int amOrPm = 0;         // pm = 1 and am = 2
        private MainWindow mw = new MainWindow(); // The parent view object

        public SetAlarm(MainWindow newMW)
        {
            this.mw = newMW; // The parent view is set 
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
            else if(day == 0)
            {
                errLbl.Content = "Please select a day (MON - SUN)";
            }
            else
            {
                
                String min = Convert.ToString(min1Lbl.Content) + Convert.ToString(min2Lbl.Content);
                /*
                    DateTime(year,month,day,hour,min,sec)
                    *day is being used for now from SUN - SAT (0 - 6)
                    *hour is being used as hour (1 - 12)
                    *min is being used as min (0 - 59)
                    *seconds is being used for now for AM and PM
                */
                DateTime test = new DateTime(2017, 2, day, Convert.ToInt32(hrLbl.Content), Convert.ToInt32(min), amOrPm);
                (this.Parent as Panel).Children.Remove(this);
                mw.setList(test);
            }
        }

        private void sunBtn_Click(object sender, RoutedEventArgs e)
        {
            day = 0;
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
            day = 1;
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
            day = 2;
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
            day = 3;
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
            day = 4;
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
            day = 5;
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
            day = 6;
            sunBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            monBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            tueBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            wedBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            thuBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            friBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            satBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));
        }
    }
}
