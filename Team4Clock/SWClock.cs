using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Team4Clock
{
    class SWClock
    {
        private List<Alarm> alarmList;
        
        public SWClock() {
            this.alarmList = new List<Alarm>();
        }

        //This this returns the current time as a string
        public String ShowTime
        {
            get
            {
                return DateTime.Now.ToString("hh:mm tt");
            }
        }

        //this is to set an alarm
        public void setAlarm(Object obj)
        {

        }

        //This to dismiss an alarm the is ringing
        public void dismissAlarm(Object obj)
        {

        }

        //This will cancel "delete" an alarm
        public void cancelAlarm(Object obj)
        {

        }

        //This will snooze a ringing alarm
        public void snoozeAlarm(Object obj)
        {

        }
    }
}
