using Play.Flies.Client.Architecture;

namespace Play.Flies.Client.Models
{
    public class ImageModel : ModelBase
    {
        private string _filename;
        private string _title;
        private string _trivia;

        public string Filename
        {
            get { return _filename; }
            set
            {
                if (_filename == value) return;
                _filename = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Trivia
        {
            get { return _trivia; }
            set
            {
                if (_trivia == value) return;
                _trivia = value;
                OnPropertyChanged();
            }
        }
    }
}