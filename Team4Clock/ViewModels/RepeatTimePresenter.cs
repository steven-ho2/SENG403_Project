using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    public class RepeatTimePresenter
    {
        private string _hours = "12";
        private string _mins = "00";
        private string _amPm = "AM";

        public string Mins
        {
            get { return _mins; }
            set { _mins = value; }
        }

        public string Hours
        {
            get { return _hours; }
            set {_hours = value; }
        }

        public string AmPm
        {
            get { return _amPm; }
            set { _amPm = value; }
        }
    }
}
