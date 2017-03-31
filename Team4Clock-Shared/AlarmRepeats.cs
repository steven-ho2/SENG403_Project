using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    /// <summary>
    /// Class for representing repetition in an alarm.
    /// Provides convenience functions for setting/unsetting repeat days,
    /// and for fetching all days for which a repeat applies. Also includes a 
    /// predicate to determine if the repeat is set for a particular day.
    /// 
    /// This class essentially wraps a dictionary with System.DayOfWeek values as
    /// keys, and internal Repeat structs as values.
    /// </summary>
    [Serializable]
    class AlarmRepeats
    {
        private Dictionary<DayOfWeek, Repeat> repeatDays;

        /// <summary>
        /// Default (and only) constructor.
        /// 
        /// Initializes dictionary of weekdays and sets all days to false -- no repeats 
        /// by default. These must be set manually.
        /// </summary>
        public AlarmRepeats()
        {
            // initialize all days to false in the default ctor (no repeats)
            repeatDays = new Dictionary<DayOfWeek, Repeat>();
            var enumeratedDays = Enum.GetValues(typeof(DayOfWeek));
            foreach (DayOfWeek day in enumeratedDays)
            {
                TimeSpan blankSpan = new TimeSpan();
                repeatDays[day] = new Repeat(false, blankSpan);
            }
        }

        /// <summary>
        /// Sets or unsets a repeat for a particular day of the week.
        /// </summary>
        /// <param name="day">DayOfWeek being altered.</param>
        /// <param name="repeats">New value of the repeat. True => repeat on this day.</param>
        /// <param name="time">TimeSpan representing the time to set this repeat for.</param>
        public void SetRepeat(DayOfWeek day, bool repeats, TimeSpan time)
        {
            repeatDays[day] = new Repeat(repeats, time);
        }

        /// <summary>
        /// Get a List of all days of the week (as DayOfWeek enum elements) for which
        /// this AlarmRepeats class applies.
        /// 
        /// This runs in O(n) time, so it's preferable to look up repeats by
        /// specific day using the RepeatsOn() function where possible.
        /// </summary>
        /// <returns>A List container, with all days of the week for which a repeat exists.</returns>
        public List<DayOfWeek> GetRepeats()
        {
            List<DayOfWeek> repeatDaysList = new List<DayOfWeek>();
            foreach (var pair in repeatDays)
            {
                if (pair.Value.repeats)
                    repeatDaysList.Add(pair.Key);
            }

            return repeatDaysList;
        }

        /// <summary>
        /// Simple predicate, determining whether a repeat is set for a particular day.
        /// </summary>
        /// <param name="day">The day of the week for which to check if a repeat exists.</param>
        /// <returns>True, if a repeat exists for "day" (i.e. if repeatDays[day] is true).</returns>
        public bool RepeatsOn(DayOfWeek day)
        {
            return repeatDays[day].repeats;
        }

        /// <summary>
        /// Gets a TimeSpan reflecting the time for which a repeat is set on a
        /// particular day.
        /// 
        /// May exhibit odd behaviour if there is no repeat set
        /// for the given day.
        /// </summary>
        /// <param name="day">The day for which a repeat time is desired.</param>
        /// <returns>The repeat time for that day.</returns>
        public TimeSpan GetRepeatForDay(DayOfWeek day)
        {
            return repeatDays[day].time;
        }

        /// <summary>
        /// Predicate to determine whether or not any repeats are set.
        /// 
        /// Runs in O(n) time, so not especially efficient.
        /// </summary>
        /// <returns>True if any repeats are set.</returns>
        public bool Repeats()
        {
            foreach (Repeat val in repeatDays.Values)
            {
                if (val.repeats) return true;
            }

            return false;
        }

        /// <summary>
        /// Basic struct to hold repeat information, allowing for variable repeats.
        /// </summary>
        [Serializable]
        protected struct Repeat
        {
            public bool repeats;
            public TimeSpan time;
            public Repeat(bool repeats, TimeSpan time) {
                this.repeats = repeats;
                this.time = time;
            }
        }
    }
}
