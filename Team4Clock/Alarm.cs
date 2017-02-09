using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    class Alarm
    {
        private bool on;
        private DateTime time;
        private Object ringtone;
        private AlarmRepeats repeatDays = new AlarmRepeats();  // Wrapper for repeat days

        //This is the contructor for the Alarm Class
        public Alarm(DateTime time)
        {
            this.time = time;
            this.on = true;
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

        //This gets the time the alarm is set to
        public DateTime getTime()
        {
            return time;
        }

        //Displays the time the alarm is set to
        public String displayTime()
        {
            return time.ToString("hh:mm tt");
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
