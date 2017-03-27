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
    /// Interaction logic for Analog.xaml
    /// </summary>
    public partial class Analog : UserControl
    {
        private MainWindow newMW = new MainWindow();

        public Analog(MainWindow newMW)
        {
            this.newMW = newMW;
            InitializeComponent();
        }

        private void setDigital(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
        }

        private void setAlarm_Click(object sender, RoutedEventArgs e)
        {
            SetAlarm newSA = new SetAlarm(0);
            (this.Parent as Panel).Children.Add(newSA);

        }
    }
}
