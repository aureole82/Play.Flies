using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Play.Flies.Client.Architecture;
using Play.Flies.Client.Models.Messages;
using Play.Flies.Client.Services;

namespace Play.Flies.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IMessageService _messenger;
        private ITabViewModel _selectedViewModel;

        public MainViewModel(IMessageService messenger, IEnumerable<ITabViewModel> viewModels)
        {
            _messenger = messenger;
            ViewModels = viewModels
                .OrderBy(tab => tab.Order)
                .ToArray();
            SelectedViewModel = ViewModels.FirstOrDefault();

            NotifyDigitKeyCommand = new RelayCommand<int>(NotifyDigitKey     /*, i => false*/);
            NotifyLetterKeyCommand = new RelayCommand<string>(NotifyLetterKey/*, i => false*/);
        }

        #region Private helper methods.

        private void NotifyDigitKey(int key)
        {
            _messenger.Publish(new DigitKeyPressedMessage(key));
        }

        private void NotifyLetterKey(string key)
        {
            _messenger.Publish(new LetterKeyPressedMessage(key));
        }

        #endregion

        #region Bindable properties and commands.

        public ITabViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                if (_selectedViewModel == value) return;

                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        public ITabViewModel[] ViewModels { get; }
        public ICommand NotifyDigitKeyCommand { get; }
        public ICommand NotifyLetterKeyCommand { get; }

        #endregion
    }
}