using System;

namespace Mediator
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatMediator mediator = new ChatMediator();

            IUser user1 = new User("Елнар");
            IUser user2 = new User("Амир");
            IUser user3 = new User("Елдос");

            mediator.AddUserToChannel(user1, "general");
            mediator.AddUserToChannel(user2, "general");
            mediator.AddUserToChannel(user3, "study");

            Console.WriteLine("\nСообщения в каналах");
            user1.Send("Всем привет", "general");

            Console.WriteLine("\nПриватное сообщение");
            user2.SendPrivate("Привет Али", "Али");

            Console.WriteLine("\nВыход пользователя");
            mediator.RemoveUserFromChannel(user1, "general");

            Console.WriteLine("\nОшибка отправки");
            user1.Send("Я уже не в канале", "general");
        }
    }
}