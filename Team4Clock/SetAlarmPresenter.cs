using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Team4Clock
{
    class SetAlarmPresenter : ObservableObject
    {
        private bool _isPm = false;
        private int _hr = 12;

        public int Hour
        {
            get { return _hr; }
            set { 
                _hr = value;
                OnPropertyChanged("Hour");
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
                _isPm = value;
            }
        }

        private readonly IEventAggregator _eventAggregator;
        public SetAlarmPresenter(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
        }

        public ICommand IncHourCommand
        {
            get { return new DelegateCommand(IncHour); }
        }
        public ICommand DecHourCommand
        {
            get { return new DelegateCommand(DecHour); }
        }

        private void IncHour()
        {
            this.Hour = (Hour + 1) % 12;
        }

        private void DecHour()
        {
            this.Hour = (Hour - 1 < 1 ? 12 : Hour - 1) ;
        }

    }
}
