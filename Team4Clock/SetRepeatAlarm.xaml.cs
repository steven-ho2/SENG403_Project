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
        // New alarm constructor
        public SetRepeatAlarm()
        {
            this.DataContext = new RepeatAlarmPresenter(ApplicationService.Instance.EventAggregator);
            InitializeComponent();
            AddEventHandlers();
        }

        // Edit constructor
        public SetRepeatAlarm(RepeatingAlarm alarm)
        {
            this.DataContext = new RepeatAlarmPresenter(alarm, ApplicationService.Instance.EventAggregator);
            InitializeComponent();
            AddEventHandlers();
        }

        private void AddEventHandlers()
        {
            // This should never be null, but better safe than sorry...
            RepeatAlarmPresenter viewModel = this.DataContext as RepeatAlarmPresenter;
            if (viewModel != null)
            {
                viewModel.NoRepeatError += ErrorEventHandler;       // handler for a "no repeats set" error event
                viewModel.SuccessEvent += ExitEvent;                // handler for a "success" event
            }
        }

        private void ErrorEventHandler(object sender, EventArgs e)
        {
            errLabel.Visibility = Visibility.Visible;
        }

        // Removes the current set alarm view from the stack allowing
        // the parent main view to be shown
        private void ExitEvent(object sender, EventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
        }
    }
}
