namespace Play.Flies.Client.Services
{
    public interface IImageServiceFactory
    {
        IImageService Create(string path);
    }
}