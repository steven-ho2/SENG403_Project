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
        private int hour;   // (->12<-):55
        private int min1;   // 12:-(>5<-)5
        private int min2;   // 12:5(->5<-)
        private int day;    // SUN = 7 MON = 1 TUE = 2 WED = 3 THU = 4 FRI = 5 SAT = 6
        private int amOrPm; // pm = 1 and am = 2

        //This is the contructor for the Alarm Class
        public Alarm(DateTime time)
        {
            this.time = time;
            this.on = true;
        }
        public Alarm(int hour, int min1, int min2, int day, int amOrPm)
        {
            this.hour   = hour;
            this.min1   = min1;
            this.min2   = min2;
            this.day    = day;
            this.amOrPm = amOrPm;
        }

        public Alarm(int hour, int min1, int min2, int day, int amOrPm)
        {
            this.hour   = hour;
            this.min1   = min1;
            this.min2   = min2;
            this.day    = day;
            this.amOrPm = amOrPm;
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

        public int getHour()
        {
            return this.hour;
        }
        public int getMin1()
        {
            return this.min1;
        }
        public int getMin2()
        {
            return this.min2;
        }
        public int getDay()
        {
            return this.day;
        }
        public int getAmorPm()
        {
            return this.amOrPm;
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
