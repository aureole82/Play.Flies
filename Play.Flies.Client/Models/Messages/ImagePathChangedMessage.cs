namespace Play.Flies.Client.Models.Messages
{
    public class ImagePathChangedMessage
    {
        public ImagePathChangedMessage(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}