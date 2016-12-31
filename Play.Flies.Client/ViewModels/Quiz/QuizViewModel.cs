using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Play.Flies.Client.Architecture;
using Play.Flies.Client.Helper;
using Play.Flies.Client.Models;
using Play.Flies.Client.Services;
using Play.Flies.Client.Services.Implementations;
using Play.Flies.Client.ViewModels.Edit;

namespace Play.Flies.Client.ViewModels.Quiz
{
    public class QuizViewModel : ViewModelBase, ITabViewModel
    {
        private readonly HashSet<string> _alreadyShownImages = new HashSet<string>();
        private readonly ContextModel _context;
        private readonly IImageServiceFactory _factory;

        [PreferredConstructor]
        public QuizViewModel(ContextModel context, IImageServiceFactory factory)
        {
            _context = context;
            _factory = factory;

            Image = new ImageViewModel(new DesignImageService(),
                new ImageModel {Filename = "test.jpg", Title = "Test", Trivia = "Trivia\r\nNext line"});

            AddPlayerCommand = new RelayCommand(AddPlayer);
            AddRowCommand = new RelayCommand(AddRow);
            RemoveRowCommand = new RelayCommand(RemoveRow, CanRemoveRow);
            AddColumnCommand = new RelayCommand(AddColumn);
            RemoveColumnCommand = new RelayCommand(RemoveColumn, CanRemoveColumn);
            CoverAllCommand = new RelayCommand(CoverAll, CanCoverAll);
            DiscoverOneCommand = new RelayCommand(DiscoverOne, CanDiscover);
            DiscoverAllCommand = new RelayCommand(DiscoverAll, CanDiscover);
            ScorePlayerCommand = new RelayCommand<int>(ScorePlayer);
            NextImageCommand = new RelayCommand(NextImage);

            FillCovers();

            AddPlayer();
        }

        public QuizViewModel() : this(null, null)
        {
            AddPlayer();
            AddPlayer();
            DiscoverOne();
            DiscoverOne();
            DiscoverOne();
        }

        #region Bindable properties and commands.

        private int _columns = 4;
        private ImageViewModel _image;
        private int _rows = 5;
        private bool _showTrivia;
        private bool _showTitle;

        public bool ShowTitle
        {
            get { return _showTitle; }
            set
            {
                if (_showTitle == value) return;

                _showTitle = value;
                OnPropertyChanged();
            }
        }

        public bool ShowTrivia
        {
            get { return _showTrivia; }
            set
            {
                if (_showTrivia == value) return;

                _showTrivia = value;
                OnPropertyChanged();
            }
        }

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

        public ImageViewModel Image
        {
            get { return _image; }
            set
            {
                if (_image == value) return;

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
        public ICommand NextImageCommand { get; }
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
            ShowTitle = false;
            ShowTrivia = false;
            foreach (var cover in Covers.Where(cover => !cover.IsCovered))
            {
                cover.IsCovered = true;
            }
        }

        private bool CanDiscover()
        {
            return !ShowTitle || !ShowTrivia || Covers.Any(cover => cover.IsCovered);
        }

        private void DiscoverAll()
        {
            var coveredTiles = Covers
                .Where(cover => cover.IsCovered)
                .ToList();

            if (!coveredTiles.Any())
            {
                if (!ShowTitle)
                {
                    ShowTitle = true;
                    return;
                }

                ShowTrivia = true;
                return;
            }

            foreach (var cover in coveredTiles)
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
            var amount = Columns*Rows;
            if (amount == Covers.Count) return;

            var addOrDelete = amount > Covers.Count
                ? new Action(() => Covers.Add(new CoverViewModel()))
                : (() => Covers.Remove(Covers.Last()));

            while (amount != Covers.Count)
            {
                addOrDelete();
            }
        }

        private void NextImage()
        {
            if (Image != null)
                _alreadyShownImages.Add(Image.Image.Filename);

            Image = null;
            var imageService = _factory.Create(_context.ImagePath);
            var nextImage = imageService.GetImages()
                .Where(model => !_alreadyShownImages.Contains(model.Filename))
                .Shuffle()
                .FirstOrDefault();
            if (nextImage == null) return;


            CoverAll();
            Image = new ImageViewModel(imageService, nextImage);
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