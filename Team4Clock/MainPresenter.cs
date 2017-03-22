using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Team4Clock
{
    public class MainPresenter : ObservableObject
    {
        private SWClock _clock = new SWClock();
        private List<Alarm> _alarmSet = new List<Alarm>();
        private DispatcherTimer _timer;

        private string _time;

        private int _snoozeDelay = -2;
        private int _setDelay = 5;
        private bool _alarmOn = false;
        string path = Assembly.GetExecutingAssembly().Location; // unnecessary reflection. should get rid of this asap
        private bool _played = false;       // what is this?
        private int _flag = 0;              // what is this?
        private AlarmUI _editThis;

        public string Time
        {
            get
            {
                return this._time;
            }
            set
            {
                if (_time == value)
                    return;
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        public int SnoozeDelay
        {
            get
            {
                return this._snoozeDelay;
            }
            set
            {
                if (_snoozeDelay == value)
                    return;
                _snoozeDelay = value;
                // OnPropertyChanged("SnoozeDelay");
            }
        }

        public MainPresenter()
        {
            StartTimer();
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Time = DateTime.Now.ToLongTimeString();

            foreach (Alarm alarm in _alarmSet)
            {
                if (DateTime.Compare(_clock.getCurrentTime(), alarm.time) == 0)
                {
                    if (alarm.on && _alarmOn == false && _played == false)
                    {
                        _played = true;
                        _player.SoundLocation = _soundLocation;
                        _player.Load();
                        _player.PlayLooping();
                        _alarmOn = true;
                        //activateSnooze();
                        alarm.Ring();
                    }
                }
            }
        }
    }
}
