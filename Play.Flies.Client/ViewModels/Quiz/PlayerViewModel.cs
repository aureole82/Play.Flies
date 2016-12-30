using System.Windows.Input;
using System.Windows.Media;
using Play.Flies.Client.Architecture;
using Play.Flies.Client.Helper;

namespace Play.Flies.Client.ViewModels.Quiz
{
    public class PlayerViewModel : ViewModelBase
    {
        public PlayerViewModel(int number = 1)
        {
            Number = number;
            _name = $"Player {number}";
            Color = Colors.Black.MixRandomColor(number);

            IncreaseScoreCommand = new RelayCommand(IncreaseScore);
        }

        #region Private helper methods.

        private void IncreaseScore()
        {
            Score++;
        }

        #endregion

        #region Bindable properties and commands.

        private string _name;
        private int _score;
        public int Number { get; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;

                _name = value;
                OnPropertyChanged();
            }
        }

        public int Score
        {
            get { return _score; }
            set
            {
                if (_score == value) return;

                _score = value;
                OnPropertyChanged();
            }
        }

        public Color Color { get; }
        public ICommand IncreaseScoreCommand { get; }

        #endregion
    }
}