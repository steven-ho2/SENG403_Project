using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Team4Clock
{
    /// <summary>
    /// ViewModel for Repeating Alarm setting interface.
    /// 
    /// Assumes that any View implementation will rely on selecting from times as list of strings
    /// (i.e. list of possible hours, list of possible minutes, list containing "AM" and "PM")
    /// for any given day of the week.
    /// 
    /// This ViewModel provides two events which should be handled by the View:
    ///     * NoRepeatError is sent when a user tries to create the alarm without any repeats.
    ///     * SuccessEvent is sent when an alarm is successfully created.
    ///     
    /// The ViewModel can handle creating a new Alarm, or editing an existing one. In the latter
    /// case, all strings are set up according to the Alarm passed in.
    /// </summary>
    public class RepeatAlarmPresenter : ObservableObject
    {
        private IEventAggregator _eventAggregator;
        private bool _isEditMode;
        private RepeatingAlarm _oldAlarm;

        // Collections of strings for use in ComboBoxes for alarm repeats. Set by InitLists().
        private ObservableCollection<string> _hrsList = new ObservableCollection<string>();
        private ObservableCollection<string> _minsList = new ObservableCollection<string>();
        private ObservableCollection<string> _amPmList = new ObservableCollection<string>();

        // RepeatTimePresenters (wrappers for selectable time strings)
        private RepeatTimePresenter _sunTimes = new RepeatTimePresenter();
        private RepeatTimePresenter _monTimes = new RepeatTimePresenter();
        private RepeatTimePresenter _tueTimes = new RepeatTimePresenter();
        private RepeatTimePresenter _wedTimes = new RepeatTimePresenter();
        private RepeatTimePresenter _thuTimes = new RepeatTimePresenter();
        private RepeatTimePresenter _friTimes = new RepeatTimePresenter();
        private RepeatTimePresenter _satTimes = new RepeatTimePresenter();

        // Booleans determining whether each individual day is selected to take a repeat
        private bool _sunCheck;
        private bool _monCheck;
        private bool _tueCheck;
        private bool _wedCheck;
        private bool _thuCheck;
        private bool _friCheck;
        private bool _satCheck;

        // Events
        public EventHandler NoRepeatError;
        public EventHandler SuccessEvent;


        // ----------------- Start of properties -----------------

        public ObservableCollection<string> HoursList
        {
            get { return _hrsList; }
            set
            {
                _hrsList = value;
                OnPropertyChanged("HoursList");
            }
        }

        public ObservableCollection<string> MinsList
        {
            get { return _minsList; }
            set
            {
                _minsList = value;
                OnPropertyChanged("MinsList");
            }
        }

        public ObservableCollection<string> AmPmList
        {
            get { return _amPmList; }
            set
            {
                _amPmList = value;
                OnPropertyChanged("AmPmList");
            }
        }

        public RepeatTimePresenter SunTimes
        {
            get { return _sunTimes; }
            set { _sunTimes = value; }
        }
        public RepeatTimePresenter MonTimes
        {
            get { return _monTimes; }
            set { _monTimes = value; }
        }
        public RepeatTimePresenter TueTimes
        {
            get { return _tueTimes; }
            set { _tueTimes = value; }
        }
        public RepeatTimePresenter WedTimes
        {
            get { return _wedTimes; }
            set { _wedTimes = value; }
        }
        public RepeatTimePresenter ThuTimes
        {
            get { return _thuTimes; }
            set { _thuTimes = value; }
        }
        public RepeatTimePresenter FriTimes
        {
            get { return _friTimes; }
            set { _friTimes = value; }
        }
        public RepeatTimePresenter SatTimes
        {
            get { return _satTimes; }
            set { _satTimes = value; }
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

        /// <summary>
        /// Base (create mode) constructor.
        /// </summary>
        /// <param name="eventAggregator">App event aggregator.</param>
        public RepeatAlarmPresenter(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            InitLists();
        }

        /// <summary>
        /// Edit mode constructor.
        /// </summary>
        /// <param name="alarm">Alarm to be edited.</param>
        /// <param name="eventAggregator">App event aggregator.</param>
        public RepeatAlarmPresenter(RepeatingAlarm alarm, IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            InitLists();
            _isEditMode = true;
            _oldAlarm = alarm;
            UpdateFromEditAlarm(alarm, DayOfWeek.Sunday, ref _sunCheck, SunTimes);
            UpdateFromEditAlarm(alarm, DayOfWeek.Monday, ref _monCheck, MonTimes);
            UpdateFromEditAlarm(alarm, DayOfWeek.Tuesday, ref _tueCheck, TueTimes);
            UpdateFromEditAlarm(alarm, DayOfWeek.Wednesday, ref _wedCheck, WedTimes);
            UpdateFromEditAlarm(alarm, DayOfWeek.Thursday, ref _thuCheck, ThuTimes);
            UpdateFromEditAlarm(alarm, DayOfWeek.Friday, ref _friCheck, FriTimes);
            UpdateFromEditAlarm(alarm, DayOfWeek.Saturday, ref _satCheck, SatTimes);

        }

        /// <summary>
        /// Method to handle setting all strings and checkbox-booleans according to
        /// the existing repeats in an alarm that has been passed in to edit.
        /// </summary>
        /// <param name="alarm">The Alarm eing edited.</param>
        /// <param name="day">The day of the week to check for repeats in the Alarm.</param>
        /// <param name="checkBox">The corresponding ViewModel boolean for this day (as ref).</param>
        /// <param name="timeInfo">The TimeInfo class for this day.</param>
        private void UpdateFromEditAlarm(RepeatingAlarm alarm, DayOfWeek day, ref bool checkBox, RepeatTimePresenter timeInfo)
        {
            if (alarm.RepeatsOn(day))
            {
                checkBox = true;
                TimeSpan span = alarm.GetRepeatForDay(day);
                int hoursNum = (span.Hours > 12) ? (span.Hours - 12) : span.Hours;
                if (hoursNum == 0) hoursNum = 12;
                string hours = hoursNum.ToString();
                string mins = span.Minutes.ToString("D2");
                timeInfo.Hours = hours;
                timeInfo.Mins = mins;
                if (span.Hours >= 12) timeInfo.AmPm = "PM";
            }
        }

        /// <summary>
        /// Initialize string lists.
        /// 
        /// Called at ViewModel initialization. Populates _hrsList with strings corresponding to the range 1-12,
        /// _minsList with strings from 00-60, and _amPmList with "AM" and "PM".
        /// </summary>
        private void InitLists()
        {
            // Set up shared string collections for combo boxes
            for (int i = 1; i <= 12; i++)
            {
                _hrsList.Add(i.ToString());
            }
            for (int i = 0; i < 60; i++)
            {
                _minsList.Add(i.ToString("D2"));
            }
            _amPmList.Add("AM");
            _amPmList.Add("PM");
        }

        /// <summary>
        /// Parse the set of strings for a given day (as a RepeatTimePresenter) into
        /// a corresponding TimeSpan.
        /// </summary>
        /// <param name="timePresenter">The RepeatTimePresenter for the given day.</param>
        /// <returns>A TimeSpan parsed from the strings in the RepeatTimePresenter.</returns>
        private TimeSpan parseRepeats(RepeatTimePresenter timePresenter)
        {
            bool setPm = (timePresenter.AmPm == "PM");
            string hoursStr = timePresenter.Hours;
            int hours = (hoursStr == "12") ? 0 : Int32.Parse(hoursStr);
            hours += setPm ? 12 : 0;
            int mins = Int32.Parse(timePresenter.Mins);
            return new TimeSpan(hours, mins, 0);
        }

        /// <summary>
        /// Sets up the repeats on a RepeatingAlarm based on parameters of
        /// the ViewModel. Essentially parses the RepeatTimePresenter for
        /// each day if the corresponding boolean is set, and then sets
        /// the appropriate repeat in the alarm.
        /// </summary>
        /// <param name="alarm">The RepeatingAlarm to set repeats for.</param>
        private void setAlarmRepeats(RepeatingAlarm alarm)
        {
            TimeSpan time;
            if (SunCheck)
            {
                time = parseRepeats(SunTimes);
                alarm.SetRepeat(DayOfWeek.Sunday, /*repeats*/ true, time);
            }
            if (MonCheck)
            {
                time = parseRepeats(MonTimes);
                alarm.SetRepeat(DayOfWeek.Monday, /*repeats*/ true, time);
            }
            if (TueCheck)
            {
                time = parseRepeats(TueTimes);
                alarm.SetRepeat(DayOfWeek.Tuesday, /*repeats*/ true, time);
            }
            if (WedCheck)
            {
                time = parseRepeats(WedTimes);
                alarm.SetRepeat(DayOfWeek.Wednesday, /*repeats*/ true, time);
            }
            if (ThuCheck)
            {
                time = parseRepeats(ThuTimes);
                alarm.SetRepeat(DayOfWeek.Thursday, /*repeats*/ true, time);
            }
            if (FriCheck)
            {
                time = parseRepeats(FriTimes);
                alarm.SetRepeat(DayOfWeek.Friday, /*repeats*/ true, time);
            }
            if (SatCheck)
            {
                time = parseRepeats(SatTimes);
                alarm.SetRepeat(DayOfWeek.Saturday, /*repeats*/ true, time);
            }
        }

        // ---------------------------- Commands ----------------------------

        /// <summary>
        /// "Done" button command. Calls Done().
        /// </summary>
        public ICommand DoneCommand
        {
            get { return new DelegateCommand(Done); }
        }


        /// <summary>
        /// "Done" function. Should be called when the user attempts to finish creating/editing
        /// a RepeatingAlarm.
        /// 
        /// If everything looks good, this will publish through the event aggregator either an
        /// EditAlarmEvent or a NewAlarmEvent (depending on create/edit mode).
        /// 
        /// Sends either a SuccessEvent on a success or a NoRepeatError in the case that this
        /// was called, but the Alarm created had no repeats set.
        /// </summary>
        private void Done()
        {
            RepeatingAlarm alarm = new RepeatingAlarm();
            setAlarmRepeats(alarm);

            if (alarm.HasRepeats())
            {
                if (_isEditMode)
                {
                    EditAlarmWrapper wrapper = new EditAlarmWrapper(_oldAlarm, alarm);
                    _eventAggregator.GetEvent<EditAlarmEvent>().Publish(wrapper);
                }
                else
                    _eventAggregator.GetEvent<NewAlarmEvent>().Publish(alarm);

                // All done, fire success event
                if (SuccessEvent != null)
                {
                    this.SuccessEvent(this, EventArgs.Empty);
                }
            }
            else
            {
                // No repeats were set; fire error event
                if (NoRepeatError != null)
                {
                    this.NoRepeatError(this, EventArgs.Empty);
                }
            }
        }
    }
}
