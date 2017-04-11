using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    /// <summary>
    /// Alarm abstract base class. Contains some methods common to all planned alarm types.
    /// </summary>
    [Serializable]
    public abstract class Alarm : IComparable 
    {

        private DateTime _snoozeTime;                                   // When snoozing: the absolute time at which the snooze period ends
        private TimeSpan _snoozeInterval;                               // The duration of a snooze for this alarm
        private bool _on;                                               // Alarm on/off state

    
        public bool on
        {
            get
            {
                return _on;
            }
            set
            {
                // cut off snoozing if we shut the alarm off
                if (value == false)
                    snoozing = false;
                _on = value;
            }
        }
        public DateTime time
        {
            get
            {
                return TruncateTime(GetTime());
            }
        }

        public string display
        {
            get { return displayTime(); }
        }
        public string info
        {
            get { return infoString(); }
        }

        public bool ringing
        {
            get;
            protected set;
        }

        public bool snoozing
        {
            get;
            private set;
        }

        public TimeSpan SnoozeInterval
        {
            get
            {
                return _snoozeInterval;
            }
            set
            {
                _snoozeInterval = value;
            }
        }

        public bool SnoozeOver
        {
            get
            {
                return ((snoozing) && (DateTime.Now > _snoozeTime));
            }
        }

        /// <summary>
        /// This gets the date and time the alarm is set to.
        /// 
        /// Implementation varies based on subclass.
        /// </summary>
        /// <returns>The date/time of the alarm.</returns>
        protected abstract DateTime GetTime();


        /// <summary>
        /// This should return a "display time", i.e. a string representing the time
        /// the Alarm is set to as it should be presented to the View.
        /// </summary>
        /// <returns>A string displaying the time of the alarm.</returns>
        public abstract String displayTime();

        /// <summary>
        /// This should return a string containing some auxiliary information about
        /// the alarm, such as repeat days for a repating alarm. Can be an empty string
        /// but must be implemented.
        /// </summary>
        /// <returns>A string with secondary info about the alarm.</returns>
        public abstract String infoString();

        /// <summary>
        /// Should the next time at which the alarm will go off, if enabled.
        /// 
        /// Semantically, the difference between this method and GetTime() is that this
        /// method should account for the possibility that the alarm is off and reflect
        /// the time the alarm will go off IF it is turned back on.
        /// </summary>
        /// <returns>The next time the alarm will go off, if the alarm is turned on.</returns>
        public abstract DateTime GetNextAlarmTime();


        /// <summary>
        /// Comparator. Compares alarms by the next alarm time (i.e. time returned by calling
        /// GetNextAlarmTime().
        /// </summary>
        /// <param name="obj">The other Alarm to compare against.</param>
        /// <returns>Standard integer reflecting comparison result.</returns>
        int IComparable.CompareTo(object obj)
        {
            Alarm a = (Alarm)obj;
            var comp = DateTime.Compare(this.GetNextAlarmTime(), a.GetNextAlarmTime());
            return comp;
        }

        /// <summary>
        /// Any internal behaviour needed when the "Wake Up" button is pressed and applied to the alarm.
        /// </summary>
        public abstract void WakeUp();

        /// <summary>
        /// Truncates DateTimes by cutting off seconds. Intended to simplify some comparisons.
        /// </summary>
        /// <param name="inTime">The time to truncate.</param>
        /// <returns>inTime, with seconds truncated. </returns>
        private DateTime TruncateTime(DateTime inTime)
        {
            TimeSpan truncatedTime = new TimeSpan(inTime.TimeOfDay.Hours,
                                                  inTime.TimeOfDay.Minutes, 0);
            DateTime date = inTime.Date;
            return date.Add(truncatedTime);
        }

        /// <summary>
        /// Ringing behaviour. Sets the "ringing" flag and disables the "snoozing" flag.
        /// </summary>
        public void Ring()
        {
            this.ringing = true;
            this.snoozing = false;
        }

        /// <summary>
        /// Snoozing behaviour. Only operates if the alarm is actually ringing, and if so,
        /// disables ringing, sets snoozing, and updates SnoozeTime.
        /// </summary>
        public void Snooze(int snzInterval)
        {
            if (ringing)
            {                
                ringing = false;
                snoozing = true;
               _snoozeInterval = new TimeSpan(0, 0, snzInterval);

               _snoozeTime = DateTime.Now + _snoozeInterval;
            }
        }
    }
}
