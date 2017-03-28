using Prism.Events;

namespace Team4Clock
{
    /// <summary>
    /// Event published when a new Alarm is created.
    /// </summary>
    public class NewAlarmEvent : PubSubEvent<Alarm>
    {
    }
}
