using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Team4Clock_Xamarin
{
    /// <summary>
    /// ViewModel for Basic Alarm setting interface.
    /// 
    /// The ViewModel can handle creating a new Alarm, or editing an existing one. In the latter
    /// case, all necessary fields are set up according to the Alarm passed in.
    /// </summary>
    public class SetAlarmViewModel : INotifyPropertyChanged
    {
        private bool _isEditMode = false;
        private Alarm _oldAlarm;
        public event PropertyChangedEventHandler PropertyChanged;

        // Fields
        private bool _isPm = false;
        private int _hr = 12;       // Hours
        private int _minTens;       // Minutes - 2nd dec. place
        private int _minOnes;       // Minutes - 1st dec. place

        public int Hour
        {
            get { return _hr; }
            set
            {
                _hr = value;
                PropertyChanged(this,new PropertyChangedEventArgs("Hour"));
            }
        }

        public int MinTens
        {
            get { return _minTens; }
            set
            {
                _minTens = value;
                PropertyChanged(this, new PropertyChangedEventArgs("MinTens"));
            }
        }

        public int MinOnes
        {
            get { return _minOnes; }
            set
            {
                _minOnes = value;
                PropertyChanged(this, new PropertyChangedEventArgs("MinOnes"));
            }
        }

        public bool IsPm
        {
            get
            {
                return _isPm;
            }
            set
            {
                System.Diagnostics.Debug.WriteLine("Helooooooooooooooooooooooooooooooooooooooooooo");
                _isPm = value;
            }
        }
       
        // ----------------------Commands----------------------

        public ICommand IncHourCommand
        {
            get { return new DelegateCommand(IncHour); }
        }
        public ICommand DecHourCommand
        {
            get { return new DelegateCommand(DecHour); }
        }

        public ICommand IncMinTensCommand
        {
            get { return new DelegateCommand(IncMinTens); }
        }

        public ICommand DecMinTensCommand
        {
            get { return new DelegateCommand(DecMinTens); }
        }

        public ICommand IncMinOnesCommand
        {
            get { return new DelegateCommand(IncMinOnes); }
        }

        public ICommand DecMinOnesCommand
        {
            get { return new DelegateCommand(DecMinOnes); }
        }

        public ICommand DoneCommand
        {
            get { return new DelegateCommand(Done); }
        }

        public ICommand IsPmCommand
        {
            get { return new DelegateCommand(IsPmConvert); }
        }

        public ICommand IsAmCommand
        {
            get { return new DelegateCommand(IsAmConvert); }
        }

        private void IncHour()
        {
            this.Hour = (Hour % 12) + 1;
        }

        private void DecHour()
        {
            this.Hour = (Hour - 1 < 1 ? 12 : Hour - 1);
        }

        private void IncMinTens()
        {
            this.MinTens = (MinTens + 1) % 6;
        }

        private void DecMinTens()
        {
            this.MinTens = (MinTens - 1 < 0 ? 5 : MinTens - 1);
        }

        private void IncMinOnes()
        {
            this.MinOnes = (MinOnes + 1) % 10;
        }

        private void DecMinOnes()
        {
            this.MinOnes = (MinOnes - 1 < 0 ? 9 : MinOnes - 1);
        }

        private void IsPmConvert()
        {
            this._isPm = true;
        }
        
        private void IsAmConvert()
        {
            this._isPm = false;
        }

        private void Done()
        {
            var mainPage = new MainPage();
            int mins = (MinTens * 10) + MinOnes;
            int hour = Hour % 12;
            hour = (IsPm) ? (hour + 12) : hour;
            hour = (hour == 24) ? 0 : hour;
            TimeSpan alarmTime = new TimeSpan(hour, mins, 0);
            BasicAlarm alarm = new BasicAlarm(alarmTime);

            if (_isEditMode)
            {
                EditAlarmWrapper wrapper = new EditAlarmWrapper(_oldAlarm, alarm);
                MessagingCenter.Send<MainPage, EditAlarmWrapper>(mainPage, "AddBasicAlarm", wrapper);
            }
            else
            {
                MessagingCenter.Send<MainPage, BasicAlarm>(mainPage, "AddBasicAlarm", alarm);
            }
                
        }
    }
}
