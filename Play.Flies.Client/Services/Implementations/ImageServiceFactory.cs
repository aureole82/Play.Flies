namespace Play.Flies.Client.Services.Implementations
{
    internal class ImageServiceFactory : IImageServiceFactory
    {
        public IImageService Create(string path)
        {
            return new ImageService(path);
        }
    }
}