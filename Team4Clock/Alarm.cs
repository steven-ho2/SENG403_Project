using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    class Alarm
    {
        private bool on = false;
        private DateTime time;
        private Object ringtone;
        private int hour;   // (->12<-):55
        private int min1;   // 12:-(>5<-)5
        private int min2;   // 12:5(->5<-)
        private int day;    // SUN = 7 MON = 1 TUE = 2 WED = 3 THU = 4 FRI = 5 SAT = 6
        private int amOrPm; // pm = 1 and am = 2

        private AlarmRepeats repeatDays = new AlarmRepeats();  // Wrapper for repeat days

        //This is the contructor for the Alarm Class
        public Alarm(DateTime time)
        {
            this.time = time;
            this.on = true;
        }
        public Alarm(int hour, int min1, int min2, int day, int amOrPm)
        {
            this.hour   = hour;
            this.min1   = min1;
            this.min2   = min2;
            this.day    = day;
            this.amOrPm = amOrPm;
        }

        //This return whether the alarm is set on or off
        public bool alarmOn()
        {
            return this.on;
        }

        //This gets the time the alarm is set to
        public DateTime getTime()
        {
            return time;
        }

        //This gets the ringtone the is set to this alarm
        public Object getRingtone()
        {
            return ringtone;
        }

        //This is to set the ringtone for the alarm
        public void setRingtone(Object obj)
        {
            this.ringtone = obj;
        }
        public int getHour()
        {
            return this.hour;
        }
        public int getMin1()
        {
            return this.min1;
        }
        public int getMin2()
        {
            return this.min2;
        }
        public int getDay()
        {
            return this.day;
        }
        public int getAmorPm()
        {
            return this.amOrPm;
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
            DateTime alarmTime = DateTime.Now.Date;
            if (currTime >= newTime)
            {
                alarmTime.AddDays(1);
            }
            alarmTime.Add(newTime);
            this.time = alarmTime;
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
            TimeSpan alarmTime = time.TimeOfDay;
            newTime.Add(alarmTime);
            this.time = newTime;
        }

        /* Set or unset a repeat for a particular day.
         * 
         * day:     The DayOfWeek for which to modify repeat behaviour.
         * repeats: Whether to repeat for this day (true: repeat for this day).
         */
        public void SetRepeat(DayOfWeek day, bool repeats)
        {
            repeatDays.SetRepeat(day, repeats);
        }

        /* Updates the alarm's time to the next instance of the repeat.
         * 
         * The actual time (TimeSpan) of the alarm is invariant in this function. All that
         * changes is the date.
         * 
         * Does nothing if no repeats are set.
         */ 
        private void UpdateRepeat()
        {
            if ((repeatDays != null) && (repeatDays.Repeats()))
            {
                DateTime now = DateTime.Now;

                // first check if the next repeat is today, and the time has
                // not yet passed for the alarm
                if (repeatDays.RepeatsOn(now.DayOfWeek))
                {
                    if (now.TimeOfDay < time.TimeOfDay)
                    {
                        SetAlarmDate(now);
                        return;
                    }
                }

                // The next instance is not today, or the time has already
                // passed. Find the next date the alarm will trigger on, and
                // reset the date.
                for (int i = 1; i < 7; i++)
                {
                    DateTime then = now.AddDays(i);
                    DayOfWeek day = then.DayOfWeek;
                    if (repeatDays.RepeatsOn(day))
                    {
                        SetAlarmDate(then);
                        return;
                    }
                }
            }
        }
    }
}
