using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    public abstract class Alarm : IComparable
    {
        public bool on
        {
            get;
            set;
        }
        public DateTime time
        {
            get {return GetTime();}
        }
        public Object ringtone
        {
            get;
            set;
        }
        public string display
        {
            get { return displayTime(); }
        }
        public string info
        {
            get { return infoString(); }
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

        public void editAlarm()
        {
            // should call the set alarm GUI and change time accordingly
        }

        public void deleteAlarm()
        {
            // should call delete from the alarm list
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
            return DateTime.Compare(this.GetNextAlarmTime(), a.GetNextAlarmTime());
        }
    }
}
