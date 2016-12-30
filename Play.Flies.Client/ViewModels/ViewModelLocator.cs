using System;
using Play.Flies.Client.Services;
using Play.Flies.Client.Services.Implementations;

namespace Play.Flies.Client.ViewModels
{
    public class ViewModelLocator : IDisposable
    {
        private readonly IContainer _container;

        public ViewModelLocator()
        {
            _container = new SimpleInjectorContainer();
            _container.Register<IMessageService, MessageService>();
            _container.Register<IImageServiceFactory, ImageServiceFactory>();
            _container.Register<MainViewModel>();
            _container.RegisterAll<ITabViewModel>();
        }

        public MainViewModel MainViewModel => _container.Resolve<MainViewModel>();

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}