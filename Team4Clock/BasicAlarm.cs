using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    /// <summary>
    /// BasicAlarm class, deriving Alarm.
    /// 
    /// This class represents a "basic alarm," i.e. one with no repeats that is instead set 
    /// to a specific time. The alarm will, by default, be set to the next occurrence of that
    /// particular time.
    /// </summary>
    public class BasicAlarm : Alarm
    {
        private DateTime alarmTime;
        
        /// <summary>
        /// Constructor which takes a TimeSpan reference, and calls SetAlarmTime().
        /// </summary>
        /// <param name="timeSpan">The TimeSpan to set the alarm to.</param>
        public BasicAlarm(TimeSpan timeSpan)
        {
            SetAlarmTime(timeSpan);
            this.on = true;
        }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public BasicAlarm()
        {
            this.on = true;
        }

        /// <summary>
        /// Sets this alarm to the next occurrence of a particular clock time, 
        /// represented by a TimeSpan reference.
        /// 
        /// Basically, this looks at the time given, and decides whether or not
        /// that time has passed for today. If not, then the alarm is set for
        /// today; otherwise, tomorrow.
        /// </summary>
        /// <param name="newTime">The alarm will be set to this time, 
        ///     for whatever day this time will next occur (today or tomorrow).</param>
        public void SetAlarmTime(TimeSpan newTime)
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

        /// <summary>
        /// Implementation of GetTime() for basic alarm.
        /// Just returns the absolute DateTime of the Alarm (may not reflect the next
        /// actual ringing time, if the alarm is currently off).
        /// </summary>
        /// <returns>The absolute DateTime of this Alarm.</returns>
        protected override DateTime GetTime()
        {
            return alarmTime;
        }

        /// <summary>
        /// Displays the time in the following format: HH:MM (AM/PM).
        /// </summary>
        /// <returns>A string reflecting the BasicAlarm's time.</returns>
        public override String displayTime()
        {
            return time.ToString("hh:mm tt");
        }

        /// <summary>
        /// Auxiliary info string override.
        /// 
        /// Currently returns an empty string as there is no additional information to give
        /// about BasicAlarms.
        /// </summary>
        /// <returns>An empty string.</returns>
        public override string infoString()
        {
            return "";
        }

        /// <summary>
        /// Override for GetNextAlarmTime(). Return value depends on whether or not the
        /// alarm is on. If the alarm is on, this just returns the time the alarm is set
        /// to.
        /// 
        /// Otherwise, it returns a DateTime where the Date is set to the next time the
        /// alarm will occur. Specifically, if the alarm's time has passed for today, it
        /// will return a DateTime with that time for tomorrow; otherwise, the date will
        /// be set for today.
        /// </summary>
        /// <returns>The next time the alarm will go off.</returns>
        public override DateTime GetNextAlarmTime()
        {
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
                DateTime today = DateTime.Today;
                DateTime tempTime = today.Add(time.TimeOfDay);
                if (DateTime.Now > tempTime)
                    return today.AddDays(1).Add(time.TimeOfDay);
                else
                    return today.Add(time.TimeOfDay);
            }
        }

        /// <summary>
        /// Wake up behaviour for a BasicAlarm. Just turns the alarm off and stops it from ringing.
        /// </summary>
        public override void WakeUp()
        {
            this.on = false;
            ringing = false;
        }
    }
}
