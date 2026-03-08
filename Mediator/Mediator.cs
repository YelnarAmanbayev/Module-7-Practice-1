using System;
using System.Collections.Generic;
using System.Linq;

namespace Mediator
{
    interface IMediator
    {
        void AddUserToChannel(IUser user, string channel);
        void RemoveUserFromChannel(IUser user, string channel);
        void SendMessage(string message, IUser sender, string channel);
        void SendPrivateMessage(string message, IUser sender, string receiver);
    }

    class ChatMediator : IMediator
    {
        private Dictionary<string, List<IUser>> channels = new Dictionary<string, List<IUser>>();

        public void AddUserToChannel(IUser user, string channel)
        {
            if (!channels.ContainsKey(channel))
            {
                channels[channel] = new List<IUser>();
                Console.WriteLine($"Канал {channel} создан");
            }

            channels[channel].Add(user);
            user.SetMediator(this);

            Notify(channel, $"{user.Name} подключился", user);
        }

        public void RemoveUserFromChannel(IUser user, string channel)
        {
            if (!channels.ContainsKey(channel)) return;

            channels[channel].Remove(user);
            Notify(channel, $"{user.Name} покинул канал", user);
        }

        public void SendMessage(string message, IUser sender, string channel)
        {
            if (!channels.ContainsKey(channel))
            {
                Console.WriteLine($"Ошибка: канал {channel} не существует");
                return;
            }

            if (!channels[channel].Contains(sender))
            {
                Console.WriteLine($"Ошибка: {sender.Name} не состоит в канале {channel}");
                return;
            }

            foreach (var user in channels[channel])
            {
                if (user != sender)
                    user.Receive($"[{channel}] {sender.Name}: {message}");
            }
        }

        public void SendPrivateMessage(string message, IUser sender, string receiver)
        {
            foreach (var channel in channels.Values)
            {
                var user = channel.FirstOrDefault(u => u.Name == receiver);

                if (user != null)
                {
                    user.Receive($"Личное сообщение от {sender.Name}: {message}");
                    return;
                }
            }

            Console.WriteLine($"Ошибка: пользователь {receiver} не найден");
        }

        private void Notify(string channel, string message, IUser excluded)
        {
            foreach (var user in channels[channel])
            {
                if (user != excluded)
                    user.Receive($"Уведомление: {message}");
            }
        }
    }
}