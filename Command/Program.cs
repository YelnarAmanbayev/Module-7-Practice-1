using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    class Program
    {
        static void Main(string[] args)
        {
            Light livingRoomLight = new Light("гостиной");
            AirConditioner airConditioner = new AirConditioner("спальне");
            TV tv = new TV("гостиной");
            SmartCurtains curtains = new SmartCurtains("спальне");
            MusicPlayer player = new MusicPlayer("гостиной");

            ICommand lightOn = new LightOnCommand(livingRoomLight);
            ICommand lightOff = new LightOffCommand(livingRoomLight);

            ICommand acOn = new AirConditionerOnCommand(airConditioner);
            ICommand acOff = new AirConditionerOffCommand(airConditioner);

            ICommand tvOn = new TVOnCommand(tv);
            ICommand tvOff = new TVOffCommand(tv);

            ICommand curtainsOpen = new CurtainsOpenCommand(curtains);
            ICommand curtainsClose = new CurtainsCloseCommand(curtains);

            ICommand musicPlay = new MusicPlayCommand(player);
            ICommand musicStop = new MusicStopCommand(player);

            RemoteControl remote = new RemoteControl();

            remote.SetCommand(1, lightOn);
            remote.SetCommand(2, lightOff);
            remote.SetCommand(3, acOn);
            remote.SetCommand(4, tvOn);
            remote.SetCommand(5, curtainsOpen);
            remote.SetCommand(6, musicPlay);

            Console.WriteLine("Проверка отдельных команд");
            remote.PressButton(1);
            remote.PressButton(3);
            remote.PressButton(4);

            Console.WriteLine("\nПроверка отмены");
            remote.UndoButton();
            remote.UndoButton();

            Console.WriteLine("\nПроверка повтора");
            remote.RedoButton();

            Console.WriteLine("\nПроверка пустого слота");
            remote.PressButton(99);

            Console.WriteLine("\nПроверка макрокоманды Вечерний режим");
            ICommand eveningMode = new MacroCommand(new List<ICommand>
            {
                lightOn,
                tvOn,
                acOn,
                curtainsClose,
                musicPlay
            });

            eveningMode.Execute();

            Console.WriteLine("\nОтмена макрокоманды");
            eveningMode.Undo();

            Console.WriteLine("\nЗапись макрокоманды с пульта");
            remote.StartRecording();
            remote.PressButton(1);
            remote.PressButton(5);
            remote.PressButton(6);
            MacroCommand recordedMacro = remote.StopRecording();

            Console.WriteLine("\nВыполнение записанной макрокоманды");
            recordedMacro.Execute();
        }
    }
}