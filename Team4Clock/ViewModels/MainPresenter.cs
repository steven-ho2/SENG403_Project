using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Team4Clock
{
    public class MainPresenter : ObservableObject
    {
        protected readonly IEventAggregator _eventAggregator;
        private SWClock _clock = new SWClock();
        private List<Alarm> _alarmSet = new List<Alarm>();
        private DispatcherTimer _timer;

        private string _time;

        private int _snoozeDelay = -2;

        public EventHandler TriggerAlarm;

        public string Time
        {
            get
            {
                return this._time;
            }
            set
            {
                if (_time == value)
                    return;
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        public int SnoozeDelay
        {
            get
            {
                return this._snoozeDelay;
            }
            set
            {
                if (_snoozeDelay == value)
                    return;
                _snoozeDelay = value;
                // OnPropertyChanged("SnoozeDelay");
            }
        }

        public MainPresenter(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            SubscribeToEvents();
            StartTimer();
        }

        private void SubscribeToEvents()
        {
            this._eventAggregator.GetEvent<NewAlarmEvent>().Subscribe((alarm) =>
            {
                _alarmSet.Add(alarm);
            });
            this._eventAggregator.GetEvent<DeleteAlarmEvent>().Subscribe((Alarm) =>
            {
                _alarmSet.Remove(Alarm);
            });
            this._eventAggregator.GetEvent<EditAlarmEvent>().Subscribe((Wrapper) =>
            {
                _alarmSet.Remove(Wrapper.OldAlarm);
                _alarmSet.Add(Wrapper.NewAlarm);
            });
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Time = DateTime.Now.ToLongTimeString();

            foreach (Alarm alarm in _alarmSet)
            {
                if (((DateTime.Compare(_clock.getCurrentTime(), alarm.time) == 0) && !alarm.snoozing)
                    || (alarm.SnoozeOver))
                {
                    if (alarm.on && !alarm.ringing)
                    {
                        alarm.Ring();

                        // Fire alarm trigger event
                        if (TriggerAlarm != null)
                        {
                            this.TriggerAlarm(this, EventArgs.Empty);
                        }
                    }
                }
            }
        }

        // ---------------- Commands ----------------

        public ICommand WakeUpCommand
        {
            get { return new DelegateCommand(WakeUpAlarms); }
        }

        public ICommand SnoozeCommand
        {
            get { return new DelegateCommand(SnoozeAlarms); }
        }

        private void WakeUpAlarms()
        {
            foreach (Alarm alarm in _alarmSet)
            {
                if (alarm.ringing)
                    alarm.WakeUp();
            }
        }

        private void SnoozeAlarms()
        {
            foreach (Alarm alarm in _alarmSet)
            {
                if (alarm.ringing)
                    alarm.Snooze();
            }
        }
    }
}
