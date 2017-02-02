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
        private bool pmClicked = false;
        private bool amClicked = false; 
        public SetAlarm(MainWindow newMW)
        {
            InitializeComponent();
        }

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
        }

        private void hrDownBtn_Click(object sender, RoutedEventArgs e)
        {
            int hr = Convert.ToInt32(hrLbl.Content);

            if (hr > 0)
            {
                hrLbl.Content = hr - 1;
            }
        }

        private void min1BtnUp_Click(object sender, RoutedEventArgs e)
        {
            int min = Convert.ToInt32(min1Lbl.Content);

            if (min < 5)
            {
                min1Lbl.Content = min + 1;
            }
        }

        private void min1DownBtn_Click(object sender, RoutedEventArgs e)
        {
            int min = Convert.ToInt32(min1Lbl.Content);

            if (min > 0)
            {
                min1Lbl.Content = min - 1;
            }
        }

        private void min2BtnUp_Click(object sender, RoutedEventArgs e)
        {
            int min = Convert.ToInt32(min2Lbl.Content);

            if (min < 9)
            {
                min2Lbl.Content = min + 1;
            }
        }

        private void min2BtnDown_Click(object sender, RoutedEventArgs e)
        {
            int min = Convert.ToInt32(min2Lbl.Content);

            if (min > 0)
            {
                min2Lbl.Content = min - 1;
            }
        }

        private void pmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!pmClicked)
            {
                pmBtn.Background = Brushes.Red;
                pmClicked = true;
                amBtn.Background = Brushes.Purple;
                amClicked = false;
            }
        }

        private void amBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!amClicked)
            {
                amBtn.Background = Brushes.Red;
                amClicked = true;
                pmBtn.Background = Brushes.Purple;
                pmClicked = false;
            }
        }

        private void doneBtn_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
        }
    }
}
