using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Team4Clock;
using Prism.Events;
using Team4Clock_Shared;

namespace Team4Clock.Mobile
{
	public partial class MainPage : ContentPage
	{
        private IEventAggregator _eventAggregator;

        private ObservableCollection<Alarm> _alarmList = new ObservableCollection<Alarm>();

        // Sound junk
        private IAudioPlayerService _audioPlayer;
        private bool _isStopped;

        public MainPage()
		{
            this._eventAggregator = ApplicationService.Instance.EventAggregator;
            
            this.BindingContext = new MainPresenter(ApplicationService.Instance.EventAggregator);
            SubscribeToEvents();
            InitializeComponent();
		}

        private void SubscribeToEvents()
        {
            // This should never be null, but better safe than sorry...
            MainPresenter viewModel = this.BindingContext as MainPresenter;
            if (viewModel != null)
            {
                viewModel.TriggerAlarm += AlarmEvent;
            }

            this._eventAggregator.GetEvent<NewAlarmEvent>().Subscribe((alarm) =>
            {
                AddAlarm(alarm);
            });

            this._eventAggregator.GetEvent<DeleteAlarmEvent>().Subscribe((alarm) =>
            {
                DeleteAlarm(alarm);
            });

            this._eventAggregator.GetEvent<EditAlarmEvent>().Subscribe((wrapper) =>
            {
                // placeholder
            });

            this._eventAggregator.GetEvent<RequestEditAlarmEvent>().Subscribe((alarm) =>
            {
                SetAlarmView();
            });
        }

        private void AddAlarm(Alarm alarm)
        {
            _alarmList.Add(alarm);
        }

        private void DeleteAlarm(Alarm alarm)
        {
            _alarmList.Remove(alarm);
        }

        private void SetAlarmBtn_Click(object sender, EventArgs e)
        {
            SetAlarmView();
        }

        private void ListBtn_Click(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
            ListAlarmView();
        }

        private void SetAlarmView(BasicAlarm alarm = null)
        {
            Navigation.PopModalAsync();
            SetAlarm setAlarm;
            setAlarm = new SetAlarm();
            Navigation.PushModalAsync(setAlarm);
        }


        private void ListAlarmView()
        {
            AlarmList alarmList = new AlarmList(_alarmList);
            Navigation.PushModalAsync(alarmList);

        }

        private void AlarmEvent(object sender, EventArgs e)
        {
            _audioPlayer = DependencyService.Get<IAudioPlayerService>();
            _audioPlayer.Play("PoliceSound.wav");
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            try
            {
                _audioPlayer.Pause();
            }
            catch(Exception ex)
            {
                // there might be an exception if the audio player isn't currently playing
                // if so, just swallow the exception
            }
            Console.WriteLine("cliiiick");
        }
    }
}
