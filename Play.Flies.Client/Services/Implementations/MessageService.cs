using System;
using System.Collections;
using System.Linq;

namespace Play.Flies.Client.Services.Implementations
{
    internal class MessageService : IMessageService
    {
        private readonly ArrayList _handlers = new ArrayList();

        public void Publish<T>(T message)
        {
            foreach (var handler in _handlers.OfType<Action<T>>())
            {
                handler(message);
            }
        }

        public void Subscribe<T>(Action<T> handler)
        {
            _handlers.Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            _handlers.Remove(handler);
        }
    }
}