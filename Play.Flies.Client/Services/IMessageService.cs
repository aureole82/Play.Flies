using System;

namespace Play.Flies.Client.Services
{
    public interface IMessageService
    {
        void Publish<T>(T message);
        void Subscribe<T>(Action<T> handler);
        void Unsubscribe<T>(Action<T> handler);
    }
}