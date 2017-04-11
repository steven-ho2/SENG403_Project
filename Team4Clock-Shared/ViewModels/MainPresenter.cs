using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Timers;

namespace Team4Clock
{
    /// <summary>
    /// ViewModel for the Main window/screen of the alarm clock application.
    /// 
    /// Mainly responsible for keeping the clock string updated, handling alarms ringing/snoozing,
    /// and maintaining the list of alarms currently in use by the application.
    /// 
    /// Any View which uses this ViewModel should be sure to handle the TriggerAlarm event,
    /// which is used by the ViewModel when an Alarm begins ringing.
    /// </summary>
    public class MainPresenter : ObservableObject
    {
        protected readonly IEventAggregator _eventAggregator;
        private SWClock _clock = new SWClock();
        private List<Alarm> _alarmSet = new List<Alarm>();
        private Timer _timer;
        private AlarmIO _alarmIO;

        private string _time;

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

        private bool _buttonsVisible;

        public bool ButtonsVisible
        {
            get
            {
                return _buttonsVisible;
            }
            set
            {
                _buttonsVisible = value;
                OnPropertyChanged("ButtonsVisible");
            }
        }

        /// <summary>
        /// ViewModel constructor.
        /// 
        /// All this ViewModel requires is an event aggregator,
        /// which should be global and should be passed in by the View.
        /// </summary>
        /// <param name="eventAggregator">Event aggregator.</param>
        public MainPresenter(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            SubscribeToEvents();
            SetupIO();
            StartTimer();
        }

        private void SetupIO()
        {
            _alarmIO = new AlarmIO();
            try
            {
                List<Alarm> testList = _alarmIO.ReadAlarmList();
                if (testList != null)
                {
                    Console.WriteLine(testList.Count);
                    _alarmSet = testList;
                    _eventAggregator.GetEvent<SetAlarmsEvent>().Publish(_alarmSet);
                }
            }

            catch (IOException)
            {
                Console.Error.WriteLine("[ERROR] Could not load alarms.bin.");
            }
        }

        /// <summary>
        /// Subscribes to events that the ViewModel needs to keep track of.
        /// 
        /// In particular, the ViewModel must subscribe to events which reflect
        /// changes to the Alarms, as it is this ViewModel which tracks the
        /// underlying list of alarms in use by the application.
        /// </summary>
        private void SubscribeToEvents()
        {
            this._eventAggregator.GetEvent<NewAlarmEvent>().Subscribe((alarm) =>
            {
                _alarmSet.Add(alarm);
                _alarmIO.WriteAlarmList(_alarmSet);
            });
            this._eventAggregator.GetEvent<DeleteAlarmEvent>().Subscribe((Alarm) =>
            {
                _alarmSet.Remove(Alarm);
                _alarmIO.WriteAlarmList(_alarmSet);
            });
            this._eventAggregator.GetEvent<EditAlarmEvent>().Subscribe((Wrapper) =>
            {
                _alarmSet.Remove(Wrapper.OldAlarm);
                _alarmSet.Add(Wrapper.NewAlarm);
                _alarmIO.WriteAlarmList(_alarmSet);
            });
        }

        /// <summary>
        /// Sets up timer details (i.e. creates DispatcherTimer, sets the interval,
        /// adds tick event, starts timer).
        /// </summary>
        private void StartTimer()
        {
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.AutoReset = true;
            _timer.Elapsed += Timer_Tick;
            _timer.Start();
        }

        /// <summary>
        /// Timer "tick" event.
        /// 
        /// At each tick, this checks for each currently set alarm if it is
        /// time for that alarm to ring (either because the time for it to 
        /// ring has been reached, or because it was snoozing and it is ready
        /// to ring again). If so, it fires a TriggerAlarm event, which should
        /// be caught by the View.
        /// 
        /// Also updates the Time string at each tick.
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event args</param>
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
                            ButtonsVisible = true;
                        }
                    }
                }
            }
        }

        // ---------------- Commands ----------------

        /// <summary>
        /// Wake Up button command.
        /// 
        /// Calls WakeUpAlarms().
        /// </summary>
        public ICommand WakeUpCommand
        {
            get { return new DelegateCommand(WakeUpAlarms); }
        }

        /// <summary>
        /// Snooze button command.
        /// 
        /// Calls SnoozeAlarms().
        /// </summary>
        public ICommand SnoozeCommand
        {
            get { return new DelegateCommand(SnoozeAlarms); }
        }

        /// <summary>
        /// Wakes up alarms by iterating through all alarms which are
        /// currently ringing, and calls the WakeUp() method on each.
        /// </summary>
        private void WakeUpAlarms()
        {
            foreach (Alarm alarm in _alarmSet)
            {
                if (alarm.ringing)
                    alarm.WakeUp();
            }
            ButtonsVisible = false;
        }

        /// <summary>
        /// Snoozes alarms by iterating through all alarms which are
        /// currently ringing, and calls the Snooze() method on each.
        /// </summary>
        private void SnoozeAlarms()
        {
            foreach (Alarm alarm in _alarmSet)
            {
                if (alarm.ringing)
                    alarm.Snooze();
            }
            ButtonsVisible = false;
        }
    }
}
