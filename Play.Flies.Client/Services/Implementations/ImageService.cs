using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Play.Flies.Client.Models;

namespace Play.Flies.Client.Services.Implementations
{
    internal class ImageService : IImageService
    {
        private const int TitleId = 0x9C9F;
        private const int CommentId = 0x9C9C;
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
            bool titleOk;
            bool commentOk;
            using (var stream = new MemoryStream())
            {
                using (var image = Image.FromFile(filename))
                {
                    titleOk = UpdateMetadata(image, TitleId, model.Title);
                    commentOk = UpdateMetadata(image, CommentId, model.Trivia);

                    if (titleOk || commentOk)
                    {
                        image.Save(stream, GetExtension(model.Filename));
                    }
                }
                File.WriteAllBytes(filename, stream.GetBuffer());
            }

            if (!titleOk)
            {
                AddMetadata(filename, TitleId, model.Title);
            }
            if (!commentOk)
            {
                AddMetadata(filename, CommentId, model.Trivia);
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
            using (var image = Image.FromFile(Path.Combine(_path, filename)))
            {
                var items = image.PropertyItems.ToDictionary(item => item.Id, item => _encoding.GetString(item.Value));
                var title = GetMetadata(items, TitleId);
                var trivia = GetMetadata(items, CommentId);

                return new ImageModel
                {
                    Filename = Path.GetFileName(filename),
                    Title = title,
                    Trivia = trivia,
                    HasChanged = false
                };
            }
        }

        private static string GetMetadata(IReadOnlyDictionary<int, string> items, int key)
        {
            string value;
            return !items.TryGetValue(key, out value) ? null : value.Replace("\0", "");
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