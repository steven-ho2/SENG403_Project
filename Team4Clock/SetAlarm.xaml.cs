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
    /// <summary>
    /// View for setting basic alarms.
    /// </summary>
    public partial class SetAlarm : UserControl
    {

        /// <summary>
        /// Default (new alarm mode) constructor.
        /// </summary>
        public SetAlarm()
        {
            //this.DataContext = new SetAlarmPresenter(ApplicationService.Instance.EventAggregator);
            this.DataContext = new HybridAlarmPresenter(ApplicationService.Instance.EventAggregator);
            InitializeComponent();
        }

        /// <summary>
        /// Edit mode constructor
        /// </summary>
        /// <param name="editAlarm">The alarm to be edited.</param>
        public SetAlarm(BasicAlarm editAlarm)
        {
            //this.DataContext = new SetAlarmPresenter(editAlarm, ApplicationService.Instance.EventAggregator);
            InitializeComponent();
        }

        public SetAlarm(HybridAlarm editAlarm)
        {
            this.DataContext = new HybridAlarmPresenter(editAlarm, ApplicationService.Instance.EventAggregator);
            InitializeComponent();
        }

        /// <summary>
        /// Removes the current set alarm view from the stack allowing
        /// the parent main view to be shown
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">Event args.</param>
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
        }
    }
}
