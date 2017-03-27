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
        private int _setDelay = 5;
        private bool _alarmOn = false;
        private bool _played = false;       // what is this?
        private int _flag = 0;              // what is this?
        private AlarmUI _editThis;

        public EventHandler TriggerAlarm;

        private ObservableCollection<Alarm> _testAlarms = new ObservableCollection<Alarm>();

        public ObservableCollection<Alarm> TestAlarms
        {
            get
            {
                return _testAlarms;
            }
            set
            {
                _testAlarms = value;
                OnPropertyChanged("TestAlarms");
            }
        }

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
            Console.WriteLine("Instantiating new MainPresenter...");
            this._eventAggregator = eventAggregator;
            this._eventAggregator.GetEvent<NewAlarmEvent>().Subscribe((alarm) => {
                _alarmSet.Add(alarm);
                TestAlarms.Add(alarm);
            });
            this._eventAggregator.GetEvent<DeleteAlarmEvent>().Subscribe((Alarm) =>
            {
                _alarmSet.Remove(Alarm);
                TestAlarms.Remove(Alarm);
            });
            StartTimer();
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
                if ((DateTime.Compare(_clock.getCurrentTime(), alarm.time) == 0)
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

        public ICommand WakeUpCommand
        {
            get { return new DelegateCommand(WakeUpAlarms); }
        }

        private void WakeUpAlarms()
        {
            foreach (Alarm alarm in _alarmSet)
            {
                if (alarm.ringing)
                    alarm.WakeUp();
            }
        }
    }
}
