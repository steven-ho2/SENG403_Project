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
    /// View for setting repeating alarms.
    /// </summary>
    public partial class SetRepeatAlarm : UserControl
    {
        /// <summary>
        /// Default (new alarm mode) constructor.
        /// </summary>
        public SetRepeatAlarm()
        {
            this.DataContext = new RepeatAlarmPresenter(ApplicationService.Instance.EventAggregator);
            InitializeComponent();
            AddEventHandlers();
        }

        /// <summary>
        /// Edit mode constructor
        /// </summary>
        /// <param name="editAlarm">The alarm to be edited.</param>
        public SetRepeatAlarm(RepeatingAlarm alarm)
        {
            this.DataContext = new RepeatAlarmPresenter(alarm, ApplicationService.Instance.EventAggregator);
            InitializeComponent();
            AddEventHandlers();
        }

        /// <summary>
        /// Sets up event handlers.
        /// The SetRepeatAlarm View needs to handle two events from the ViewModel:
        ///     * NoRepeatError is handled by displaying an error message and blocking attempts to save the alarm.
        ///     * SuccessEvent is handled by just exiting the View.
        /// </summary>
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

        /// <summary>
        /// Event handler for a NoRepeatError event, given by the ViewModel.
        /// 
        /// Shows the error label with pre-defined contents.
        /// </summary>
        /// <param name="sender">Sending obj</param>
        /// <param name="e">Event args</param>
        private void ErrorEventHandler(object sender, EventArgs e)
        {
            errLabel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Removes the current set alarm view from the stack allowing
        /// the parent main view to be shown
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">Event args.</param>
        private void ExitEvent(object sender, EventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
        }
    }
}
