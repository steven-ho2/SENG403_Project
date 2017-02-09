using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    /*  AlarmRepeats Class
     * 
     *  Class for representing repetition in an alarm.
     *  Provides convenience functions for setting/unsetting repeat days,
     *  and for fetching all days for which a repeat applies. Also includes a 
     *  predicate to determine if the repeat is set for a particular day.
     *  
     *  This class essentially wraps a dictionary with System.DayOfWeek values as
     *  keys, and booleans as values (false: no repeat; true: repeat).
     */

    class AlarmRepeats
    {
        private Dictionary<DayOfWeek, bool> repeatDays;

        /* Default (and only) constructor.
         * Initializes dictionary of weekdays and sets all days to false -- no repeats 
         * by default.
         */
        public AlarmRepeats()
        {
            // initialize all days to false in the default ctor (no repeats)
            repeatDays = new Dictionary<DayOfWeek, bool>();
            var enumeratedDays = Enum.GetValues(typeof(DayOfWeek));
            foreach (DayOfWeek day in enumeratedDays)
            {
                repeatDays[day] = false;
            }
        }

        /* Sets or unsets a repeat for a particular day of the week, according
         * to "repeats".
         * 
         * day:     DayOfWeek being altered.
         * repeats: new value of the repeat.
         *              true -> repeat on this day
         *              false -> do not repeat on this day
         */
        public void SetRepeat(DayOfWeek day, bool repeats)
        {
            repeatDays[day] = repeats;
        }


        /* Get a List of all days of the week (as DayOfWeek enum elements) for which
         * this AlarmRepeats class applies.
         * 
         * This runs in O(n) time, so it's preferable to look up repeats by
         * specific day using the RepeatsOn() function where possible.
         * 
         * Return: a List container, with all days of the week for which a repeat exists
         *          (i.e. for which the dictionary value is true).
         */
        public List<DayOfWeek> GetRepeats()
        {
            List<DayOfWeek> repeatDaysList = new List<DayOfWeek>();
            foreach (var pair in repeatDays)
            {
                if (pair.Value)
                    repeatDaysList.Add(pair.Key);
            }

            return repeatDaysList;
        }

        /* Simple predicate, determining whether a repeat is set for a particular day.
         * 
         * day:     the day of the week for which to check if a repeat exists.
         * 
         * Return: true, if a repeat exists for "day" (i.e. if repeatDays[day] is true).
         */
        public bool RepeatsOn(DayOfWeek day)
        {
            return repeatDays[day];
        }

        /* Predicate to determine whether or not any repeats are set.
         * 
         * Runs in O(n) time, so not especially efficient.
         */
        public bool Repeats()
        {
            foreach (bool val in repeatDays.Values)
            {
                if (val) return true;
            }

            return false;
        }
    }
}
