using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Team4Clock
{
    /// <summary>
    /// ViewModel for AlarmUI controls.
    /// 
    /// Contains view interaction logic for this control.
    /// </summary>
    class AlarmUIPresenter : ObservableObject
    {
        private Alarm _alarm;
        private IEventAggregator _eventAggregator;

        public Alarm Alarm {
            get { return _alarm; }
            set
            {
                _alarm = value;
                OnPropertyChanged("Alarm");
            }
        }

        public bool AlarmOn
        {
            get { return _alarm.on; }
            set
            {
                _alarm.on = value;
                OnPropertyChanged("AlarmOn");
            }
        }

        /// <summary>
        /// Constructor. Requires an Alarm (because this ViewModel has no meaning without one)
        /// and an EventAggregator for event publishing.
        /// </summary>
        /// <param name="alarm">Alarm to represent with this ViewModel.</param>
        /// <param name="eventAggregator">Event aggregator. Should be global.</param>
        public AlarmUIPresenter(Alarm alarm, IEventAggregator eventAggregator)
        {
            this._alarm = alarm;
            this._eventAggregator = eventAggregator;
        }


        // ----------------------Commands----------------------

        /// <summary>
        /// Command for pushing the "Delete Alarm" button, or equivalent.
        /// 
        /// Ultimate publishes a DeleteAlarmEvent for the alarm this AlarmUI
        /// represents.
        /// </summary>
        public ICommand DelAlarmCommand
        {
            get { return new DelegateCommand(DeleteAlarm); }
        }

        /// <summary>
        /// Command for pushing the "Edit Alarm" button, or equivalent.
        /// 
        /// Ultimately publishes a RequestEditAlarmEvent for the alarm this
        /// AlarmUI represents.
        /// </summary>
        public ICommand EditAlarmCommand
        {
            get { return new DelegateCommand(EditAlarm); }
        }


        private void DeleteAlarm()
        {
            _eventAggregator.GetEvent<DeleteAlarmEvent>().Publish(_alarm);
        }

        private void EditAlarm()
        {
            _eventAggregator.GetEvent<RequestEditAlarmEvent>().Publish(_alarm);
        }
    }
}
