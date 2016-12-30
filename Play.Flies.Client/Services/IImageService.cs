using System.Collections.Generic;
using Play.Flies.Client.Models;

namespace Play.Flies.Client.Services
{
    public interface IImageService
    {
        IEnumerable<ImageModel> GetImages();
        void Update(ImageModel image);
        byte[] GeneratePreview(ImageModel image);
        void Revert(ImageModel image);
    }
}