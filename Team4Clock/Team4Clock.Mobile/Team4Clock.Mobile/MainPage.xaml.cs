using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Team4Clock;
using Prism.Events;

namespace Team4Clock.Mobile
{
	public partial class MainPage : ContentPage
	{
        private IEventAggregator _eventAggregator;

        private ObservableCollection<Alarm> _tempAlarmList = new ObservableCollection<Alarm>();

		public MainPage()
		{
            this._eventAggregator = ApplicationService.Instance.EventAggregator;
            SubscribeToEvents();
            this.BindingContext = new MainPresenter(ApplicationService.Instance.EventAggregator);
			InitializeComponent();
		}

        private void SubscribeToEvents()
        {
            this._eventAggregator.GetEvent<NewAlarmEvent>().Subscribe((alarm) =>
            {
                AddAlarm(alarm);
            });

            this._eventAggregator.GetEvent<DeleteAlarmEvent>().Subscribe((alarm) =>
            {
                // placeholder
            });


            this._eventAggregator.GetEvent<EditAlarmEvent>().Subscribe((wrapper) =>
            {
                // placeholder
            });

            this._eventAggregator.GetEvent<RequestEditAlarmEvent>().Subscribe((alarm) =>
            {
                // placeholder
            });

            this._eventAggregator.GetEvent<SetAlarmsEvent>().Subscribe((alarmSet) =>
            {
                // placeholder
            });
        }

        private void AddAlarm(Alarm alarm)
        {
            _tempAlarmList.Add(alarm);
        }


        private void SetAlarmBtn_Click(object sender, EventArgs e)
        {
            SetAlarmView();
        }

        private void ListBtn_Click(object sender, EventArgs e)
        {
            ListAlarmView();
        }

        private void SetAlarmView(BasicAlarm alarm = null)
        {
            SetAlarm setAlarm;
            setAlarm = new SetAlarm();
            Navigation.PushModalAsync(setAlarm);
        }


        private void ListAlarmView()
        {
            AlarmList alarmList = new AlarmList(_tempAlarmList);
            Navigation.PushModalAsync(alarmList);
        }
    }
}
