using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Team4Clock
{
    public class NewAlarmEvent : PubSubEvent<Alarm>
    {
    }
}
