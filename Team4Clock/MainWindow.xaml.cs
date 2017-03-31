using System;
using System.Media;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Prism.Events;

namespace Team4Clock
{
    /// <summary>
    /// The View for the MainWindow.
    /// 
    /// This class represents pretty much all of the UI for the Windows implementation of this application.
    /// Other controls/Views are added or taken away from this window as needed.
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {

        // Alarm UI properties and containers, including a map to map Alarms to their representative AlarmUIs.
        private ObservableCollection<AlarmUI> collection;
        private Dictionary<Alarm, AlarmUI> _alarmUIMap = new Dictionary<Alarm,AlarmUI>();

        public ObservableCollection<AlarmUI> Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Collection"));
                }
            }
        }

        // Sound data
        private SoundPlayer _player = new SoundPlayer();
        private string _soundLocation = @"PoliceSound.wav";

        // Event fields
        public event PropertyChangedEventHandler PropertyChanged;
        private IEventAggregator _eventAggregator;

        /// <summary>
        /// MainWindow constructor. Must be parameterless.
        /// 
        /// Sets up event handling, and initializes AlarmUI collection.
        /// </summary>
        public MainWindow()
        {
            Collection = new ObservableCollection<AlarmUI>();

            this._eventAggregator = ApplicationService.Instance.EventAggregator;
            SubscribeToEvents();

            this.DataContext = new MainPresenter(ApplicationService.Instance.EventAggregator);

            InitializeComponent();
            this.KeyUp += MainWindow_KeyUp;

            // This should never be null, but better safe than sorry...
            MainPresenter viewModel = this.DataContext as MainPresenter;
            if (viewModel != null)
            {
                viewModel.TriggerAlarm += AlarmEvent;
            }
        }

        /// <summary>
        /// Subscribe to Prism (MVVM aggregated) events.
        /// 
        /// MainWindow must listen to NewAlarm, DeleteAlarm, EditAlarm, and RequestEditAlarm events
        /// to update the View in accordance with these changes.
        /// </summary>
        private void SubscribeToEvents()
        {
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
                DeleteAlarm(wrapper.OldAlarm);
                AddAlarm(wrapper.NewAlarm);
            });

            this._eventAggregator.GetEvent<RequestEditAlarmEvent>().Subscribe((alarm) =>
            {
                OpenEditAlarm(alarm);
            });

            this._eventAggregator.GetEvent<SetAlarmsEvent>().Subscribe((alarmSet) =>
            {
                foreach(Alarm alarm in alarmSet)
                {
                    AddAlarm(alarm);
                }
            });
        }

        /// <summary>
        /// Add a new Alarm to the View.
        /// 
        /// Creates a new AlarmUI and adds it to the collection (and also to the map which
        /// allows tracking of the Alarm it is connected to).
        /// </summary>
        /// <param name="alarm">The alarm for which an AlarmUI is to be made.</param>
        private void AddAlarm(Alarm alarm)
        {
            AlarmUI alarmUI = new AlarmUI(alarm);
            _alarmUIMap.Add(alarm, alarmUI);
            Collection.Add(alarmUI);
        }

        /// <summary>
        /// Removes an Alarm from the View.
        /// 
        /// Looks up the given Alarm in the map, and removes its corresponding AlarmUI,
        /// as well as the map entry itself.
        /// </summary>
        /// <param name="alarm">The Alarm that is being removed.</param>
        private void DeleteAlarm(Alarm alarm)
        {
            AlarmUI delAlarmUI = _alarmUIMap[alarm];
            Collection.Remove(delAlarmUI);
            _alarmUIMap.Remove(alarm);
        }

        /// <summary>
        /// Open up the editing View for a given Alarm. 
        /// 
        /// Behaviour depends on the type of Alarm given. This method determines which
        /// subclass the Alarm belongs to and calls the appropriate SetView method.
        /// </summary>
        /// <param name="alarm">The alarm to edit.</param>
        private void OpenEditAlarm(Alarm alarm)
        {
            Type alarmType = alarm.GetType();
            if (alarmType == typeof(BasicAlarm))
                SetAlarmView(alarm as BasicAlarm);
            else
                SetRepeatView(alarm as RepeatingAlarm);
        }

        /// <summary>
        /// Opens the View for SetAlarm, either in New Alarm mode (default) or
        /// in Edit mode (if a BasicAlarm is passed).
        /// </summary>
        /// <param name="alarm">The alarm to edit. If null, then create a new alarm.</param>
        private void SetAlarmView(BasicAlarm alarm = null)
        {
            SetAlarm setAlarm;
            if (alarm == null)
                // set new alarm
                setAlarm = new SetAlarm();
            else
                // edit alarm
                setAlarm = new SetAlarm(alarm);
            Main.Children.Add(setAlarm);
        }

        /// <summary>
        /// Opens the View for SetRepeatingAlarm, either in New Alarm mode (default) or
        /// in Edit mode (if a RepeatingAlarm is passed).
        /// </summary>
        /// <param name="alarm">The alarm to edit. If null, then create a new alarm.</param>
        private void SetRepeatView(RepeatingAlarm alarm = null)
        {
            SetRepeatAlarm setAlarm;
            if (alarm == null)
                // set new alarm
                setAlarm = new SetRepeatAlarm();
            else
                //edit alarm
                setAlarm = new SetRepeatAlarm(alarm);
            Main.Children.Add(setAlarm);
        }

        /// <summary>
        /// Key event handler for the Escape key. 
        /// 
        /// Shuts down the app if Esc is pressed.
        /// </summary>
        /// <param name="sender">Sending object (ignored)</param>
        /// <param name="e">Event arguments (ignored)</param>
        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Event handler for when an alarm goes off.
        /// 
        /// Plays alarm sound and shows hidden "wake up/snooze" buttons.
        /// </summary>
        /// <param name="sender">Sending object (ignored)</param>
        /// <param name="e">Event args (ignored)</param>
        private void AlarmEvent(object sender, EventArgs e)
        {
            _player.SoundLocation = _soundLocation;
            _player.Load();
            _player.PlayLooping();
            ShowWakeUpButtons();            
        }

        /// <summary>
        /// Click handler for clicking either Wake Up or Snooze.
        /// 
        /// Stops the playing alarm sound and hides Wake Up/Snooze buttons.
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event args</param>
        private void stop_Click(object sender, RoutedEventArgs e)
        {
            _player.Stop();

           snoozeButton.Visibility = Visibility.Hidden;
           awakeButton.Visibility = Visibility.Hidden;
        }
        
        /// <summary>
        /// Activate the "Wake Up" and "Snooze" buttons
        /// </summary>
        public void ShowWakeUpButtons()
        {
            snoozeButton.Visibility = Visibility.Visible;
            awakeButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Click handler for the "Set Alarm" button.
        /// 
        /// Just calls SetAlarmView() (create new alarm mode).
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Event args.</param>
        private void setAlarmBtn_Click(object sender, RoutedEventArgs e)
        {
            SetAlarmView();
        }

        /// <summary>
        /// Click handler for the "Set Repeating Alarm" button.
        /// 
        /// Just calls SetRepeatView() (create new alarm mode).
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Event args.</param>
        private void rptAlarmBtn_Click(object sender, RoutedEventArgs e)
        {
            SetRepeatView();
        }

        /// <summary>
        /// Analog toggle button event handler. 
        /// 
        /// Currently switches the view to the "Analog" window.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Event args.</param>
        private void toggleBtn_Click(object sender, RoutedEventArgs e)
        {
            Analog analog = new Analog(this);
            Main.Children.Add(analog);
        }
    }
}
