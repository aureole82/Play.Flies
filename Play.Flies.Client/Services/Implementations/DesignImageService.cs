using System;
using System.Collections.Generic;
using Play.Flies.Client.Models;
using Play.Flies.Client.Resources;

namespace Play.Flies.Client.Services.Implementations
{
    public class DesignImageService : IImageService
    {
        public IEnumerable<ImageModel> GetImages()
        {
            yield return new ImageModel
            {
                Filename = "Sample.jpg",
                Title = "Lord Sample",
                Trivia = "† 1900 Jan 1st"
            };
            yield return new ImageModel
            {
                Filename = "Example.jpg",
                Title = "Duchess Example",
                Trivia = "† 1899 Dec 31st"
            };
            yield return new ImageModel
            {
                Filename = "Test.jpg",
                Title = "Sir Test",
                Trivia = "† 1899 Dec 30th"
            };
        }

        public void Update(ImageModel image)
        {
        }

        public byte[] GeneratePreview(ImageModel image)
        {
            return Convert.FromBase64String(Constants.Placeholder);
        }

        public void Revert(ImageModel image)
        {
        }
    }
}