using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Play.Flies.Client.Architecture;
using Play.Flies.Client.Services;

namespace Play.Flies.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ITabViewModel _selectedViewModel;

        public MainViewModel(IMessageService messenger, IEnumerable<ITabViewModel> viewModels)
        {
            ViewModels = viewModels
                .OrderBy(tab => tab.Order)
                .ToArray();
            SelectedViewModel = ViewModels.FirstOrDefault();
        }

        #region Private helper methods.

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