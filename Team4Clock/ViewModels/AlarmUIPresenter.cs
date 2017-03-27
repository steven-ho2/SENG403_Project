using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Team4Clock
{
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

        public AlarmUIPresenter(Alarm alarm, IEventAggregator eventAggregator)
        {
            this._alarm = alarm;
            this._eventAggregator = eventAggregator;
        }

        //----------Commands-------------

        public ICommand DelAlarmCommand
        {
            get { return new DelegateCommand(DeleteAlarm); }
        }

        public ICommand ToggleAlarmCommand
        {
            get { return new DelegateCommand(ToggleAlarm); }
        }


        private void DeleteAlarm()
        {
            _eventAggregator.GetEvent<DeleteAlarmEvent>().Publish(_alarm);
        }

        private void ToggleAlarm()
        {
            _alarm.toggleAlarmOn();
        }

    }
}
