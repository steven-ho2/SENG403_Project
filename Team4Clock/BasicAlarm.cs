using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    class BasicAlarm : Alarm
    {
        private DateTime alarmTime;

       //This is the contructor for the BasicAlarm Class
        public BasicAlarm(DateTime time)
        {
            this.alarmTime = time;
            this.on = true;
        }
        
        // Alternate constructor which takes a TimeSpan reference,
        // and calls SetAlarmTime().
        public BasicAlarm(TimeSpan timeSpan)
        {
            SetAlarmTime(timeSpan);
            this.on = true;
        }

        /* Sets this alarm to the next occurrence of a particular clock time, 
         * represented by a TimeSpan reference.
         * 
         * Basically, this looks at the time given, and decides whether or not
         * that time has passed for today. If not, then the alarm is set for
         * today; otherwise, tomorrow.
         * 
         * newTime: The alarm will be set to this time, for whatever day this time 
         *          will next occur (today or tomorrow).
         */
        private void SetAlarmTime(TimeSpan newTime)
        {
            TimeSpan currTime = DateTime.Now.TimeOfDay;
            DateTime newAlarmTime = DateTime.Now.Date;
            if (currTime >= newTime)
            {
                newAlarmTime = newAlarmTime.AddDays(1);
            }
            newAlarmTime += newTime;
            this.alarmTime = newAlarmTime;
        }

        protected override DateTime GetTime()
        {
            return alarmTime;
        }

        /* Sets this alarm to a specific date.
         * 
         * Ignores the time component of the provided DateTime and keeps whatever
         * time of day is currently defined for this alarm.
         * 
         * date: DateTime object containing the new date to use. Time of day is ignored.
         */
        private void SetAlarmDate(DateTime date)
        {
            DateTime newTime = date.Date;        // new date, with time reset to midnight
            TimeSpan newSpan = time.TimeOfDay;
            newTime += newSpan;
            this.alarmTime = newTime;
        }

        /* Sets this alarm to a specific date and time.
         * 
         * dateTime: the date and time to set the alarm for
         */
        private void SetAlarm(DateTime dateTime)
        {
            this.alarmTime = dateTime;
        }

        public override String displayTime()
        {
            return time.ToString("hh:mm tt");
        }

        public override string infoString()
        {
            return "";  // no additional info needed for a basic alarm at this time
        }

        public override DateTime GetNextAlarmTime()
        {
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;

            // Alarm is enabled.
            // Return whatever date/time is set for the alarm.
            if (this.on)
            {
                return time;
            }
            // Alarm is disabled.
            // Return the alarm's time for the next day the time will occur (either today or tomorrow).
            else
            {
                return today.Add(time.TimeOfDay);
            }
        }

        public override void WakeUp()
        {
            this.on = false;
            this.alarmTime = this.alarmTime.AddHours(1);
            Console.WriteLine("New time: " + alarmTime);
            ringing = false;
        }
    }
}
