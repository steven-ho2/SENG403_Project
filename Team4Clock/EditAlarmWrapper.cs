using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    struct EditAlarmWrapper
    {
        public Alarm OldAlarm, NewAlarm;

        public EditAlarmWrapper(Alarm oldAlarm, Alarm newAlarm)
        {
            OldAlarm = oldAlarm;
            NewAlarm = newAlarm;
        }
    }
}
