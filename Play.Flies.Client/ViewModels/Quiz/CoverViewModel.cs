using Play.Flies.Client.Architecture;

namespace Play.Flies.Client.ViewModels.Quiz
{
    public class CoverViewModel : ViewModelBase
    {
        private bool _isCovered = true;

        public bool IsCovered
        {
            get { return _isCovered; }
            set
            {
                if (_isCovered == value) return;

                _isCovered = value;
                OnPropertyChanged();
            }
        }
    }
}