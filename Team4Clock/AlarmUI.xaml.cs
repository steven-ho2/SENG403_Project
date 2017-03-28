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
    /// View for AlarmUIs on Windows desktop.
    /// </summary>
    public partial class AlarmUI : UserControl
    {

        /// <summary>
        /// Constructor. Requires an Alarm as input (because the UI control has no meaning without one).
        /// 
        /// Sets DataContext to ViewModel and initializes the control.
        /// </summary>
        /// <param name="inputAlarm">The Alarm this AlarmUI is meant to represent.</param>
        public AlarmUI(Alarm inputAlarm)
        {
            DataContext = new AlarmUIPresenter(inputAlarm, ApplicationService.Instance.EventAggregator);
            InitializeComponent();
        }
    }
}
