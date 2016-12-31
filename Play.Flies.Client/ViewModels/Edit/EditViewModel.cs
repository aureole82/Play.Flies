using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Play.Flies.Client.Architecture;
using Play.Flies.Client.Helper;
using Play.Flies.Client.Models;
using Play.Flies.Client.Services;
using Play.Flies.Client.Services.Implementations;

namespace Play.Flies.Client.ViewModels.Edit
{
    public class EditViewModel : ViewModelBase, ITabViewModel
    {
        private readonly ContextModel _context;
        private readonly IImageServiceFactory _factory;
        private IImageService _service;

        [PreferredConstructor]
        public EditViewModel(ContextModel context, IImageServiceFactory factory)
        {
            _context = context;
            _imagePath = context.ImagePath;
            _factory = factory;
            _service = factory.Create(ImagePath);
            LoadImagesCommand = new RelayCommand(LoadImages);

            LoadImages();
        }

        public EditViewModel() : this(null, new DesignImageServiceFactory())
        {
        }

        #region Private helper methods.

        private void LoadImages()
        {
            Images.Clear();
            foreach (var image in _service.GetImages())
            {
                Images.Add(new ImageViewModel(_service, image));
            }
        }

        #endregion

        #region Bindable properties and commands.

        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if (_imagePath == value) return;
                _imagePath = value;

                _service = _factory.Create(ImagePath);
                _context.ImagePath = _imagePath;
                LoadImages();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ImageViewModel> Images { get; } = new ObservableCollection<ImageViewModel>();
        public string Name { get; } = "Edit";
        public int Order { get; } = 2;

        public ICommand LoadImagesCommand { get; }

        #endregion
    }

    public class ImageViewModel : ViewModelBase
    {
        private readonly IImageService _service;

        public ImageViewModel(IImageService service, ImageModel image)
        {
            _service = service;
            Image = image;
            Preview = _service.GeneratePreview(image).ToImage();
            UpdateCommand = new RelayCommand(Update, CanUpdate);
            RevertCommand = new RelayCommand(Revert, CanRevert);
        }

        #region Private helper methods.

        private bool CanUpdate()
        {
            return Image.HasChanged;
        }

        private void Update()
        {
            _service.Update(Image);
        }

        private void Revert()
        {
            _service.Revert(Image);
        }

        private bool CanRevert()
        {
            return Image.HasChanged;
        }

        #endregion

        #region Bindable properties and commands.

        private BitmapImage _preview;
        public ImageModel Image { get; }

        public BitmapImage Preview
        {
            get { return _preview; }
            set
            {
                if (Equals(_preview, value)) return;

                _preview = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateCommand { get; }
        public ICommand RevertCommand { get; }

        #endregion
    }
}