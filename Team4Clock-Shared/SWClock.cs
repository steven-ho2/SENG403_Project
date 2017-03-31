using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Team4Clock
{
    /// <summary>
    /// Clock class.
    /// 
    /// This class has little utility in the current iteration of the app, and is only
    /// used to get the current time in truncated format.
    /// 
    /// This should be deprecated soon.
    /// </summary>
    class SWClock
    {
        /// <summary>
        /// Gets the current time, with seconds truncated.
        /// </summary>
        /// <returns>The current time, with seconds truncated.</returns>
        public DateTime getCurrentTime()
        {
            DateTime dt = DateTime.Now;
            DateTime now = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute,0);
            return now;
        }

    }
}
