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
        private int flag = 0;             // flag is edit alarm or create new alarm

        public SetAlarm(int setFlag)
        {
            this.flag = setFlag;
            this.DataContext = new SetAlarmPresenter(ApplicationService.Instance.EventAggregator);
            InitializeComponent();
        }

        // Edit mode constructor
        public SetAlarm(BasicAlarm editAlarm)
        {
            this.DataContext = new SetAlarmPresenter(editAlarm, ApplicationService.Instance.EventAggregator);
            InitializeComponent();
        }

        // Removes the current set alarm view from the stack allowing
        // the parent main view to be shown
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
        }
    }
}
