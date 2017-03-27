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
        bool _hasError = true;

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
                viewModel.NoRepeatError += ErrorEventHandler;
                viewModel.SuccessEvent += SuccessEventHandler;
            }
        }

        private void ErrorEventHandler(object sender, EventArgs e)
        {
            errLabel.Visibility = Visibility.Visible;
        }

        private void SuccessEventHandler(object sender, EventArgs e)
        {
            CloseWindow();
        }

        // Removes the current set alarm view from the stack allowing
        // the parent main view to be shown
        private void back_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            (this.Parent as Panel).Children.Remove(this);
        }
    }
}
