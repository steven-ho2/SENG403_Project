using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Team4Clock_Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetAlarm : ContentPage
    {
        public SetAlarm()
        {
            InitializeComponent();
        }

        private async void DoneButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }   
    }
}
