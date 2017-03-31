using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team4Clock
{
    /// <summary>
    /// Basically just a class to wrap a trio of strings, used to reflect
    /// repeats as defined by the RepeatAlarmPresenter ViewModel.
    /// 
    /// This was originally intended to be a ViewModel in its own right, thus
    /// the name and placement. Now, however, it is more like a struct. The 
    /// naming is kept as-is because I'm lazy.
    /// </summary>
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
