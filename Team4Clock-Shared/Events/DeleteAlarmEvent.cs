using Prism.Events;

namespace Team4Clock
{
    /// <summary>
    /// Event published when an Alarm is deleted.
    /// </summary>
    class DeleteAlarmEvent : PubSubEvent<Alarm>
    {
    }
}
