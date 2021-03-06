﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    /// <summary>
    /// RepeatingAlarm class, deriving Alarm.
    /// 
    /// This class represents an Alarm which repeats on certain days as long as it is on.
    /// Repeats are based on days of the week, and may apply to different times on different
    /// days.
    /// </summary>
    [Serializable]
    public class RepeatingAlarm : Alarm
    {
        private AlarmRepeats repeatDays = new AlarmRepeats();  // Wrapper for repeat days
        private DateTime lastTrigger;

        /// <summary>
        /// Default and only constructor.
        /// 
        /// Initializes a RepeatingAlarm with no Repeats. While semantically this makes
        /// little sense, it is expected that repeats should be set dynamically.
        /// </summary>
        public RepeatingAlarm()
        {
            this.on = true;
        }

        /// <summary>
        /// Sets or unsets a repeat for a particular day, and particular time.
        /// </summary>
        /// <param name="day">The DayOfWeek for which to modify repeat behaviour.</param>
        /// <param name="repeats">True -> repeat on this day.</param>
        /// <param name="time">The time to repeat on this day.</param>
        public void SetRepeat(DayOfWeek day, bool repeats, TimeSpan time)
        {
            repeatDays.SetRepeat(day, repeats, time);
        }

        /// <summary>
        /// Predicate, determining if the Alarm repeats on the given day.
        /// </summary>
        /// <param name="day">The day for which to check if a repeat exists.</param>
        /// <returns>True if the alarm repeats on this day.</returns>
        public bool RepeatsOn(DayOfWeek day)
        {
            return repeatDays.RepeatsOn(day);
        }

        /// <summary>
        /// Gets the time at which the alarm repeats on a given day.
        /// </summary>
        /// <param name="day">The day for which to check a repeat.</param>
        /// <returns>The time at which the alarm repeats on the given day.</returns>
        public TimeSpan GetRepeatForDay(DayOfWeek day)
        {
            return repeatDays.GetRepeatForDay(day);
        }

        /// <summary>
        /// GetTime() override. For RepeatingAlarm, this just calls 
        /// GetNextAlarmTime().
        /// </summary>
        /// <returns>The result of GetNextAlarmTime().</returns>
        protected override DateTime GetTime()
        {
            return GetNextAlarmTime();
        }

        /// <summary>
        /// Determine the time of the next repeat, regardless of whether the alarm is enabled.
        /// </summary>
        /// <returns>The next date/time this alarm will go off, if it is enabled.</returns>
        public override DateTime GetNextAlarmTime()
        {
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;

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

        /// <summary>
        /// Predicate: does this RepeatingAlarm actually repeat on any day at all?
        /// </summary>
        /// <returns>True if the Alarm has any repeats set whatsoever.</returns>
        public bool HasRepeats()
        {
            return repeatDays.Repeats();
        }

        /// <summary>
        /// Displays the next time of the alarm in the format "[Day] HH:MM (AM/PM)"
        /// </summary>
        /// <returns>A string containing the weekday and time of the next repeat.</returns>
        public override String displayTime()
        {
            return time.ToString("ddd hh:mm tt");
        }

        /// <summary>
        /// Auxiliary info string.
        /// 
        /// Displays a string containing abbreviated days of the week for which the alarm repeats.
        /// </summary>
        /// <returns>A list of days the alarm repeats on, delimited by spaces.</returns>
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

        /// <summary>
        /// Wake-up logic for RepeatingAlarms.
        /// 
        /// Turns off ringing and updates the last trigger time to today (to prevent
        /// the possibility of erroneously updating the repeat to the same day).
        /// </summary>
        public override void WakeUp()
        {
            ringing = false;
            lastTrigger = DateTime.Today;
        }

    }
}
