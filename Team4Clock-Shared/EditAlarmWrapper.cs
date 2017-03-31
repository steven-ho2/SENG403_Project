using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    /// <summary>
    /// This struct is a wrapper for editing alarms.
    /// 
    /// The intent of this class is to act as a workaround to allow the EditAlarmEvent to carry
    /// two alarms as a payload. Maintaining a reference to the old alarm (the one we want to edit) and
    /// the new alarm (the alarm post-editing) allows us to get around having to edit alarms in-place,
    /// which can be unwieldy and messy.
    /// </summary>
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
