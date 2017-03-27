using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    public class RepeatingAlarm : Alarm
    {
        private AlarmRepeats repeatDays = new AlarmRepeats();  // Wrapper for repeat days
        private DateTime lastTrigger;

        public RepeatingAlarm()
        {
            this.on = true;
        }

        /* Set or unset a repeat for a particular day.
         * 
         * day:     The DayOfWeek for which to modify repeat behaviour.
         * repeats: Whether to repeat for this day (true: repeat for this day).
         */
        public void SetRepeat(DayOfWeek day, bool repeats, TimeSpan time)
        {
            repeatDays.SetRepeat(day, repeats, time);
        }

        // Debug method to list out all repeat days with the times for each repeat.
        // Returns a string with this information.
        public string ListRepeatDaysAndTimes()
        {
            string retStr = "";

            List<DayOfWeek> days = repeatDays.GetRepeats();
            foreach (DayOfWeek day in days)
            {
                TimeSpan time = repeatDays.GetRepeatForDay(day);
                retStr += day + ": " + time + "\n";
            }

            return retStr;
        }

        protected override DateTime GetTime()
        {
            return GetNextAlarmTime();
        }

        public override DateTime GetNextAlarmTime()
        {
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;

            // Return the time of the next repeat, regardless of whether the alarm is enabled.

            // first check if there's a repeat for today, and if we've passed it or not
            if ((lastTrigger != DateTime.Today) 
                && (repeatDays.RepeatsOn(now.DayOfWeek)))
            {
                TimeSpan rptTime = repeatDays.GetRepeatForDay(now.DayOfWeek);
                if (now.TimeOfDay <= rptTime.Add(new TimeSpan(0,1,0)))
                {
                    return today.Add(rptTime);
                }
            }

            // otherwise find the next repeat
            for (int i = 1; i <= 7; i++)
            {
                DateTime nextDay = today.AddDays(i);
                if (repeatDays.RepeatsOn(nextDay.DayOfWeek))
                {
                    return nextDay.Add(repeatDays.GetRepeatForDay(nextDay.DayOfWeek));
                }
            }

            // failing that (should be impossible), just return the "default" time
            return time;
        }

        public bool HasRepeats()
        {
            return repeatDays.Repeats();
        }

        public override String displayTime()
        {
            return time.ToString("ddd hh:mm tt");
        }

        public override string infoString()
        {
            string rptString = "Repeats: ";

            foreach (DayOfWeek dwk in Enum.GetValues(typeof(DayOfWeek)))
            {
                if (repeatDays.RepeatsOn(dwk))
                {
                    rptString += dwk.ToString().Substring(0, 2) + " ";
                }
            }

            return rptString;
        }

        public override void WakeUp()
        {
            ringing = false;
            lastTrigger = DateTime.Today;
        }

    }
}
