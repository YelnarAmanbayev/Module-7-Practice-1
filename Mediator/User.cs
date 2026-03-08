using System;

namespace Mediator
{
    interface IUser
    {
        string Name { get; }
        void Send(string message, string channel);
        void SendPrivate(string message, string receiver);
        void Receive(string message);
        void SetMediator(IMediator mediator);
    }

    class User : IUser
    {
        public string Name { get; private set; }
        private IMediator mediator;

        public User(string name)
        {
            Name = name;
        }

        public void SetMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public void Send(string message, string channel)
        {
            mediator.SendMessage(message, this, channel);
        }

        public void SendPrivate(string message, string receiver)
        {
            mediator.SendPrivateMessage(message, this, receiver);
        }

        public void Receive(string message)
        {
            Console.WriteLine($"{Name} получил: {message}");
        }
    }
}