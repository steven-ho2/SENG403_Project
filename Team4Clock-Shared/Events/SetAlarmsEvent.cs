using Prism.Events;
using System.Collections.Generic;

namespace Team4Clock
{
    /// <summary>
    /// Event published when an Alarm is deleted.
    /// </summary>
    class SetAlarmsEvent : PubSubEvent<List<Alarm>>
    {
    }
}
