using Play.Flies.Client.Architecture;

namespace Play.Flies.Client.Models
{
    public class ContextModel : ObservableModel
    {
        private string _imagePath = @"Images";

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if (_imagePath == value) return;

                _imagePath = value;
                OnPropertyChanged();
            }
        }
    }
}