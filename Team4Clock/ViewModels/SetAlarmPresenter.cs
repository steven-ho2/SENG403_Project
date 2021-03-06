﻿using Prism.Events;
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
    class SetAlarmPresenter : ObservableObject
    {
        private bool _isEditMode = false;
        private Alarm _oldAlarm;

        // Fields
        private bool _isPm = false;
        private int _hr = 12;       // Hours
        private int _minTens;       // Minutes - 2nd dec. place
        private int _minOnes;       // Minutes - 1st dec. place

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

        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Base (create mode) constructor
        /// </summary>
        /// <param name="eventAggregator">Event aggregator for the app</param>
        public SetAlarmPresenter(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Edit mode constructor
        /// </summary>
        /// <param name="alarm">Alarm to be edited.</param>
        /// <param name="eventAggregator">Event aggregator for the app</param>
        public SetAlarmPresenter(BasicAlarm alarm, IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            _isEditMode = true;
            _oldAlarm = alarm;
            int oldMins = _oldAlarm.time.Minute;
            _minTens = oldMins / 10;
            _minOnes = oldMins % 10;
            
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
            BasicAlarm alarm = new BasicAlarm(alarmTime);

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
