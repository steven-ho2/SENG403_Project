using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Team4Clock.Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SetAlarm : ContentPage
	{
		public SetAlarm ()
		{
            this.BindingContext = new HybridAlarmPresenter(ApplicationService.Instance.EventAggregator);
            InitializeComponent ();
		}

        public SetAlarm(HybridAlarm alarm)
        {
            this.BindingContext = new HybridAlarmPresenter(alarm, ApplicationService.Instance.EventAggregator);
            InitializeComponent();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
            //(this.Parent as Panel).Children.Remove(this);
        }
    }
}
