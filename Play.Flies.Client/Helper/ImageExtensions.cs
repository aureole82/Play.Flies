using System.IO;
using System.Windows.Media.Imaging;

namespace Play.Flies.Client.Helper
{
    public static class ImageExtensions
    {
        public static BitmapImage ToImage(this byte[] raw)
        {
            var image = new BitmapImage();
            image.BeginInit();
            var stream = new MemoryStream(raw);
            image.StreamSource = stream;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            stream.Dispose();
            return image;
        }
    }
}