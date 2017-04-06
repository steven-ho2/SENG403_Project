using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Team4Clock_Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListAlarm : ContentPage
    {
        public ObservableCollection<BasicAlarm> collections { get; set; }
        public ListAlarm(ObservableCollection<BasicAlarm> newCollections)
        {
            collections = newCollections;
            InitializeComponent();
            listView.ItemsSource = collections;
        }
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            // Deselect row
            listView.SelectedItem = null;

            //await Navigation.PushModalAsync(new DetailPage(e.SelectedItem));
            await Navigation.PushAsync(new DetailPage(e.SelectedItem));
        }
        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            BasicAlarm see = (BasicAlarm) mi.CommandParameter;
            collections.Remove(see);
            DisplayAlert("Delete Action", "Successfully Deleted Alarm", "OK");
        }
    }
}
