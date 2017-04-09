using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Team4Clock_Xamarin
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<BasicAlarm> collections { get; set; }
        public MainPage()
        {
            collections = new ObservableCollection<BasicAlarm>();
            InitializeComponent();
            SubscribeToEvents();
        }

        private async void SetAlarmButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SetAlarm());
        }
        private async void ListAlarmButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ListAlarm(collections));
        }
        private void SubscribeToEvents()
        {
            MessagingCenter.Subscribe<MainPage, BasicAlarm>(this, "AddBasicAlarm", (sender, args) =>
            {
                DisplayAlert("Success Notification", "Sucessfully Added Alarm", "OK");
                collections.Add(args);
            });
        }

    }
}
