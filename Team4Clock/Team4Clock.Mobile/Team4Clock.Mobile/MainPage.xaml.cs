using System;
using System.Collections.Generic;
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

		public MainPage()
		{
            this._eventAggregator = ApplicationService.Instance.EventAggregator;
            this.BindingContext = new MainPresenter(ApplicationService.Instance.EventAggregator);
			InitializeComponent();
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
            AlarmList alarmList = new AlarmList();
            Navigation.PushModalAsync(alarmList);
        }
    }
}
