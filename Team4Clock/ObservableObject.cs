using System.ComponentModel;

namespace Team4Clock
{
    /// <summary>
    /// Convenience class for a quick default implementation of INotifyPropertyChanged.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Basic handler for a Property Changed handler.
        /// 
        /// Simply call this method with a property name string to post a
        /// PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
