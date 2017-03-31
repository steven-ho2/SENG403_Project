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
	}
}
