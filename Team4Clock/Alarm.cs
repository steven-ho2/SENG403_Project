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

        //This is the contructor for the Alarm Class
        public Alarm(DateTime time)
        {
            this.time = time;
            this.on = true;
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
    }
}
