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
    public partial class AlarmUI : UserControl
    {
        public AlarmUI(Alarm inputAlarm)
        {
            DataContext = new AlarmUIPresenter(inputAlarm, ApplicationService.Instance.EventAggregator);
            InitializeComponent();
        }

        // TODO: resolve this
        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            //a.editAlarm();
            //mw.editFromListAlarm(this, a);
        }
    }
}
