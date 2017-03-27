using Prism.Events;

namespace Team4Clock
{
    /// <summary>
    /// Event published when an Alarm has just finished being edited.
    /// </summary>
    class EditAlarmEvent : PubSubEvent<EditAlarmWrapper>
    {
    }
}
