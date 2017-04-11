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
    /// ViewModel for Basic Alarm setting interface.
    /// 
    /// The ViewModel can handle creating a new Alarm, or editing an existing one. In the latter
    /// case, all necessary fields are set up according to the Alarm passed in.
    /// </summary>
    class HybridAlarmPresenter : ObservableObject
    {
        private bool _isEditMode = false;
        private HybridAlarm _oldAlarm;

        // Fields
        private bool _isPm = false;
        private int _hr = 12;       // Hours
        private int _minTens;       // Minutes - 2nd dec. place
        private int _minOnes;       // Minutes - 1st dec. place

        // Booleans determining whether each individual day is selected to take a repeat
        private bool _sunCheck;
        private bool _monCheck;
        private bool _tueCheck;
        private bool _wedCheck;
        private bool _thuCheck;
        private bool _friCheck;
        private bool _satCheck;

        public int Hour
        {
            get { return _hr; }
            set { 
                _hr = value;
                OnPropertyChanged("Hour");
            }
        }

        public int MinTens
        {
            get { return _minTens; }
            set
            {
                _minTens = value;
                OnPropertyChanged("MinTens");
            }
        }

        public int MinOnes
        {
            get { return _minOnes; }
            set
            {
                _minOnes = value;
                OnPropertyChanged("MinOnes");
            }
        }

        public bool IsPm
        {
            get
            {
                return _isPm;
            }
            set
            {
                _isPm = value;
            }
        }


        public bool SunCheck
        {
            get { return _sunCheck; }
            set { _sunCheck = value; OnPropertyChanged("SunCheck"); }
        }

        public bool MonCheck
        {
            get { return _monCheck; }
            set { _monCheck = value; }
        }

        public bool TueCheck
        {
            get { return _tueCheck; }
            set { _tueCheck = value; }
        }

        public bool WedCheck
        {
            get { return _wedCheck; }
            set { _wedCheck = value; }
        }

        public bool ThuCheck
        {
            get { return _thuCheck; }
            set { _thuCheck = value; }
        }

        public bool FriCheck
        {
            get { return _friCheck; }
            set { _friCheck = value; }
        }

        public bool SatCheck
        {
            get { return _satCheck; }
            set { _satCheck = value; }
        }

        // ----------------- End of properties -----------------

        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Base (create mode) constructor
        /// </summary>
        /// <param name="eventAggregator">Event aggregator for the app</param>
        public HybridAlarmPresenter(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Edit mode constructor
        /// </summary>
        /// <param name="alarm">Alarm to be edited.</param>
        /// <param name="eventAggregator">Event aggregator for the app</param>
        public HybridAlarmPresenter(HybridAlarm alarm, IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            _isEditMode = true;
            _oldAlarm = alarm;
            int oldMins = _oldAlarm.time.Minute;
            _minTens = oldMins / 10;
            _minOnes = oldMins % 10;
            int oldHours = _oldAlarm.time.Hour;
            if (oldHours >= 12)
            {
                oldHours -= 12;
                _isPm = true;
            }
            if (oldHours == 0) oldHours = 12;
            _hr = oldHours;

            if (alarm.HasRepeats())
            {
                // set checkbox vars
                if (alarm.RepeatsOn(DayOfWeek.Sunday))
                    SunCheck = true;
                if (alarm.RepeatsOn(DayOfWeek.Monday))
                    MonCheck = true;
                if (alarm.RepeatsOn(DayOfWeek.Tuesday))
                    TueCheck = true;
                if (alarm.RepeatsOn(DayOfWeek.Wednesday))
                    WedCheck = true;
                if (alarm.RepeatsOn(DayOfWeek.Thursday))
                    ThuCheck = true;
                if (alarm.RepeatsOn(DayOfWeek.Friday))
                    FriCheck = true;
                if (alarm.RepeatsOn(DayOfWeek.Saturday))
                    SatCheck = true;
            }
        }


        private void SetAlarmRepeats(HybridAlarm alarm)
        {
            if (SunCheck)
            {
                alarm.SetRepeat(DayOfWeek.Sunday, /*repeats*/ true);
            }
            if (MonCheck)
            {
                alarm.SetRepeat(DayOfWeek.Monday, /*repeats*/ true);
            }
            if (TueCheck)
            {
                alarm.SetRepeat(DayOfWeek.Tuesday, /*repeats*/ true);
            }
            if (WedCheck)
            {
                alarm.SetRepeat(DayOfWeek.Wednesday, /*repeats*/ true);
            }
            if (ThuCheck)
            {
                alarm.SetRepeat(DayOfWeek.Thursday, /*repeats*/ true);
            }
            if (FriCheck)
            {
                alarm.SetRepeat(DayOfWeek.Friday, /*repeats*/ true);
            }
            if (SatCheck)
            {
                alarm.SetRepeat(DayOfWeek.Saturday, /*repeats*/ true);
            }
        }

        // ----------------------Commands----------------------

        public ICommand IncHourCommand
        {
            get { return new DelegateCommand(IncHour); }
        }
        public ICommand DecHourCommand
        {
            get { return new DelegateCommand(DecHour); }
        }

        public ICommand IncMinTensCommand
        {
            get { return new DelegateCommand(IncMinTens); }
        }

        public ICommand DecMinTensCommand
        {
            get { return new DelegateCommand(DecMinTens); }
        }

        public ICommand IncMinOnesCommand
        {
            get { return new DelegateCommand(IncMinOnes); }
        }

        public ICommand DecMinOnesCommand
        {
            get { return new DelegateCommand(DecMinOnes); }
        }

        public ICommand DoneCommand
        {
            get { return new DelegateCommand(Done); }
        }

        private void IncHour()
        {
            this.Hour = (Hour % 12) + 1;
        }

        private void DecHour()
        {
            this.Hour = (Hour - 1 < 1 ? 12 : Hour - 1) ;
        }

        private void IncMinTens()
        {
            this.MinTens = (MinTens + 1) % 6;
        }

        private void DecMinTens()
        {
            this.MinTens = (MinTens - 1 < 0 ? 5 : MinTens - 1);
        }
        private void IncMinOnes()
        {
            this.MinOnes = (MinOnes + 1) % 10;
        }

        private void DecMinOnes()
        {
            this.MinOnes = (MinOnes - 1 < 0 ? 9 : MinOnes - 1);
        }

        /// <summary>
        /// "Done" button functionality.
        /// 
        /// Parses the various fields in use by the ViewModel into a TimeSpan, and creates an
        /// Alarm with the given time. Then, publishes either an EditAlarmEvent or a NewAlarmEvent,
        /// depending on mode.
        /// </summary>
        private void Done()
        {
            int mins = (MinTens * 10) + MinOnes;
            int hour = Hour % 12;
            hour = (IsPm) ? (hour + 12) : hour;
            hour = (hour == 24) ? 0 : hour;
            TimeSpan alarmTime = new TimeSpan(hour, mins, 0);
            HybridAlarm alarm = new HybridAlarm(alarmTime);

            if (SunCheck)
                alarm.SetRepeat(DayOfWeek.Sunday, true);
            if (MonCheck)
                alarm.SetRepeat(DayOfWeek.Monday, true);
            if (TueCheck)
                alarm.SetRepeat(DayOfWeek.Tuesday, true);
            if (WedCheck)
                alarm.SetRepeat(DayOfWeek.Wednesday, true);
            if (ThuCheck)
                alarm.SetRepeat(DayOfWeek.Thursday, true);
            if (FriCheck)
                alarm.SetRepeat(DayOfWeek.Friday, true);
            if (SatCheck)
                alarm.SetRepeat(DayOfWeek.Saturday, true);

            if (_isEditMode)
            {
                EditAlarmWrapper wrapper = new EditAlarmWrapper(_oldAlarm, alarm);
                _eventAggregator.GetEvent<EditAlarmEvent>().Publish(wrapper);
            }
            else
                _eventAggregator.GetEvent<NewAlarmEvent>().Publish(alarm);
        }
    }
}
