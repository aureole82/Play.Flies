namespace Play.Flies.Client.Services.Implementations
{
    public class DesignImageServiceFactory : IImageServiceFactory
    {
        public IImageService Create(string path)
        {
            return new DesignImageService();
        }
    }
}