using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using ImageMagick;
using Play.Flies.Client.Models;

namespace Play.Flies.Client.Services.Implementations
{
    internal class ImageService : IImageService
    {
        private readonly Encoding _encoding = Encoding.UTF7;
        private readonly string _path;

        public ImageService(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            _path = path;
        }

        public IEnumerable<ImageModel> GetImages()
        {
            return Directory.GetFiles(_path, "*.jpg")
                .Concat(Directory.GetFiles(_path, "*.png"))
                .Select(Load);
        }

        public void Update(ImageModel model)
        {
            var filename = Path.Combine(_path, model.Filename);

            using (var stream = new MemoryStream())
            {
                using (var image = new MagickImage(filename))
                {
                    var profile = new ExifProfile();
                    profile.SetValue(ExifTag.ImageDescription, model.Title);
                    profile.SetValue(ExifTag.XPComment, _encoding.GetBytes(model.Trivia));
                    image.AddProfile(profile);

                    image.Write(stream);
                }
                File.WriteAllBytes(filename, stream.GetBuffer());
            }

            model.HasChanged = false;
        }

        public byte[] GeneratePreview(ImageModel image)
        {
            var imagepath = Path.Combine(_path, image.Filename);
            return File.ReadAllBytes(imagepath);
        }

        public void Revert(ImageModel image)
        {
            var stored = Load(image.Filename);
            image.Filename = stored.Filename;
            image.Title = stored.Title;
            image.Trivia = stored.Trivia;
            image.HasChanged = false;
        }

        #region Private helper methods.

        private static ImageFormat GetExtension(string filename)
        {
            var extension = Path.GetExtension(filename)?.ToLower();
            if (extension == null) return null;

            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
            }
            return ImageFormat.Bmp;
        }

        private ImageModel Load(string filename)
        {
            using (var image = new MagickImage(filename))
            {
                var profile = image.GetExifProfile();
                var title = profile?.GetValue(ExifTag.ImageDescription)?.Value as string;
                var trivia = profile?.GetValue(ExifTag.XPComment)?.Value as byte[];

                return new ImageModel
                {
                    Filename = Path.GetFileName(filename),
                    Title = title,
                    Trivia = _encoding.GetString(trivia ?? new byte[0]),
                    HasChanged = false
                };
            }
        }

        private static bool UpdateMetadata(Image image, int key, string value)
        {
            var propertyItem = image.PropertyItems.FirstOrDefault(item => item.Id == key);
            if (propertyItem == null) return false;

            propertyItem.Type = 1;
            propertyItem.Value = Encoding.UTF7.GetBytes(value ?? string.Empty);
            propertyItem.Len = propertyItem.Value.Length;
            image.SetPropertyItem(propertyItem);
            return true;
        }

        public void AddMetadata(string filename, int key, string value)
        {
            // TODO!
        }

        #endregion
    }
}