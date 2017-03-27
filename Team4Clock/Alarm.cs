using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    public abstract class Alarm : IComparable 
    {

        private DateTime _snoozeTime;
        private TimeSpan _snoozeInterval = new TimeSpan(0, 0, 5);
        private bool _on;

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

        

        //This return whether the alarm is set on or off
        public bool toggleAlarmOn()
        {
            if(this.on)
            {
                this.on = false;
                return this.on;
            }
            else
            {
                this.on = true;
                return this.on;
            }
        }

        // This gets the time the alarm is set to
        protected abstract DateTime GetTime();

        public bool checkAlarm(DateTime cur)
        {
            return this.time.Equals(cur);
        }

        //Displays the time the alarm is set to
        public abstract String displayTime();

        // Returns some secondary information about the alarm
        public abstract String infoString();

        public abstract DateTime GetNextAlarmTime();


        // Comparator. Compares next time of alarm for both alarms (see GetNextAlarmTime() above).
        int IComparable.CompareTo(object obj)
        {
            Alarm a = (Alarm)obj;
            var comp = DateTime.Compare(this.GetNextAlarmTime(), a.GetNextAlarmTime());
            return comp;
        }

        public abstract void WakeUp();

        private DateTime TruncateTime(DateTime inTime)
        {
            TimeSpan truncatedTime = new TimeSpan(inTime.TimeOfDay.Hours,
                                                  inTime.TimeOfDay.Minutes, 0);
            DateTime date = inTime.Date;
            return date.Add(truncatedTime);
        }

        public void Ring()
        {
            this.ringing = true;
            this.snoozing = false;
        }

        public void Snooze()
        {
            if (ringing)
            {                
                ringing = false;
                snoozing = true;
                _snoozeTime = DateTime.Now + _snoozeInterval;
            }
        }
    }
}
