using System.ComponentModel;

namespace Play.Flies.Client.Architecture
{
    public class ModelBase : ObservableModel
    {
        private bool _hasChanged;

        public ModelBase()
        {
            PropertyChanged += MarkHasChanged;
        }

        public bool HasChanged
        {
            get { return _hasChanged; }
            set
            {
                if (_hasChanged == value) return;
                _hasChanged = value;
                OnPropertyChanged();
            }
        }

        private void MarkHasChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HasChanged)) return;

            HasChanged = true;
        }

        ~ModelBase()
        {
            PropertyChanged -= MarkHasChanged;
        }
    }
}