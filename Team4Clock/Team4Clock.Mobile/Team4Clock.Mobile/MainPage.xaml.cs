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


        private void setAlarmBtn_Click(object sender, EventArgs e)
        {
            SetAlarmView();
        }

        private void SetAlarmView(BasicAlarm alarm = null)
        {
            SetAlarm setAlarm;
            setAlarm = new SetAlarm();
            Navigation.PushModalAsync(setAlarm);
        }
    }
}
