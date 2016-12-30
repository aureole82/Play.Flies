using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Play.Flies.Client.Architecture;
using Play.Flies.Client.Helper;
using Play.Flies.Client.Models.Messages;
using Play.Flies.Client.Resources;
using Play.Flies.Client.Services;

namespace Play.Flies.Client.ViewModels.Quiz
{
    public class QuizViewModel : ViewModelBase, ITabViewModel
    {
        [PreferredConstructor]
        public QuizViewModel(IMessageService messenger)
        {
            var bytes = Convert.FromBase64String(Constants.Placeholder);
            Image = bytes.ToImage();

            AddPlayerCommand = new RelayCommand(AddPlayer);
            AddRowCommand = new RelayCommand(AddRow);
            RemoveRowCommand = new RelayCommand(RemoveRow, CanRemoveRow);
            AddColumnCommand = new RelayCommand(AddColumn);
            RemoveColumnCommand = new RelayCommand(RemoveColumn, CanRemoveColumn);
            CoverAllCommand = new RelayCommand(CoverAll, CanCoverAll);
            DiscoverOneCommand = new RelayCommand(DiscoverOne, CanDiscover);
            DiscoverAllCommand = new RelayCommand(DiscoverAll, CanDiscover);
            ScorePlayerCommand=new RelayCommand<int>(ScorePlayer);

            FillCovers();

            AddPlayer();
        }

        public QuizViewModel() : this(null)
        {
            AddPlayer();
            AddPlayer();
            DiscoverOne();
            DiscoverOne();
            DiscoverOne();
        }

        #region Bindable properties and commands.

        private int _columns = 4;
        private BitmapImage _image;
        private int _rows = 5;

        public int Columns
        {
            get { return _columns; }
            set
            {
                if (_columns == value) return;

                _columns = value;
                FillCovers();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CoverViewModel> Covers { get; } = new ObservableCollection<CoverViewModel>();
        public ObservableCollection<PlayerViewModel> Players { get; } = new ObservableCollection<PlayerViewModel>();

        public BitmapImage Image
        {
            get { return _image; }
            set
            {
                if (Equals(_image, value)) return;

                _image = value;
                OnPropertyChanged();
            }
        }

        public int Rows
        {
            get { return _rows; }
            set
            {
                if (_rows == value) return;

                _rows = value;
                FillCovers();
                OnPropertyChanged();
            }
        }

        public ICommand AddPlayerCommand { get; }
        public ICommand AddRowCommand { get; }
        public ICommand RemoveRowCommand { get; }
        public ICommand AddColumnCommand { get; }
        public ICommand RemoveColumnCommand { get; }
        public ICommand CoverAllCommand { get; }
        public ICommand DiscoverOneCommand { get; }
        public ICommand DiscoverAllCommand { get; }
        public ICommand ScorePlayerCommand { get; }

        public string Name { get; } = "Quiz";
        public int Order { get; } = 1;

        #endregion

        #region Private helper methods.

        private void AddPlayer()
        {
            Players.Add(new PlayerViewModel(Players.Count + 1));
        }

        private void AddColumn()
        {
            Columns++;
        }

        private void AddRow()
        {
            Rows++;
        }

        private bool CanCoverAll()
        {
            return Covers.Any(cover => !cover.IsCovered);
        }

        private void CoverAll()
        {
            foreach (var cover in Covers.Where(cover => !cover.IsCovered))
            {
                cover.IsCovered = true;
            }
        }

        private bool CanDiscover()
        {
            return Covers.Any(cover => cover.IsCovered);
        }

        private void DiscoverAll()
        {
            foreach (var cover in Covers.Where(cover => cover.IsCovered))
            {
                cover.IsCovered = false;
            }
        }

        private void DiscoverOne()
        {
            var toDiscover = Covers
                .Where(cover => cover.IsCovered)
                .Shuffle()
                .First();
            toDiscover.IsCovered = false;
        }

        private void FillCovers()
        {
            var amount = Columns * Rows;
            if (amount == Covers.Count) return;

            var addOrDelete = amount > Covers.Count
                ? new Action(() => Covers.Add(new CoverViewModel()))
                : (() => Covers.Remove(Covers.Last()));

            while (amount != Covers.Count)
            {
                addOrDelete();
            }
        }

        private void ScorePlayer(int number)
        {
            Players.FirstOrDefault(player => player.Number == number)?.IncreaseScoreCommand.Execute(null);
        }
        
        private bool CanRemoveColumn()
        {
            return Columns > 1;
        }

        private void RemoveColumn()
        {
            Columns--;
        }

        private bool CanRemoveRow()
        {
            return Rows > 1;
        }

        private void RemoveRow()
        {
            Rows--;
        }

        #endregion
    }
}