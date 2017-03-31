using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    /// <summary>
    /// Event published when the application requests that an Alarm be
    /// edited.
    /// </summary>
    public class RequestEditAlarmEvent : PubSubEvent<Alarm>
    {
    }
}
