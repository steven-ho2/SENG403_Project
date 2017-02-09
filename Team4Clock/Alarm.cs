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
    }
}
