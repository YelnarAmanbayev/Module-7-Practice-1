using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    // 1. Общий интерфейс команд
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    // Null Object для пустого слота
    public class NoCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Для этого слота команда не назначена.");
        }

        public void Undo()
        {
            Console.WriteLine("Отменять нечего.");
        }
    }

    // 2. Устройства
    public class Light
    {
        public string Location { get; }

        public Light(string location)
        {
            Location = location;
        }

        public void On() => Console.WriteLine($"Свет в {Location} включен.");
        public void Off() => Console.WriteLine($"Свет в {Location} выключен.");
    }

    public class AirConditioner
    {
        public string Location { get; }
        public int Temperature { get; private set; } = 24;
        public bool IsOn { get; private set; }

        public AirConditioner(string location)
        {
            Location = location;
        }

        public void On()
        {
            IsOn = true;
            Console.WriteLine($"Кондиционер в {Location} включен. Температура: {Temperature}°C");
        }

        public void Off()
        {
            IsOn = false;
            Console.WriteLine($"Кондиционер в {Location} выключен.");
        }

        public void SetTemperature(int temperature)
        {
            Temperature = temperature;
            Console.WriteLine($"Кондиционер в {Location}: установлена температура {Temperature}°C");
        }
    }

    public class TV
    {
        public string Location { get; }
        public bool IsOn { get; private set; }

        public TV(string location)
        {
            Location = location;
        }

        public void On()
        {
            IsOn = true;
            Console.WriteLine($"Телевизор в {Location} включен.");
        }

        public void Off()
        {
            IsOn = false;
            Console.WriteLine($"Телевизор в {Location} выключен.");
        }
    }

    // Дополнительные устройства
    public class SmartCurtains
    {
        public string Location { get; }

        public SmartCurtains(string location)
        {
            Location = location;
        }

        public void Open() => Console.WriteLine($"Шторы в {Location} открыты.");
        public void Close() => Console.WriteLine($"Шторы в {Location} закрыты.");
    }

    public class MusicPlayer
    {
        public string Location { get; }
        public bool IsPlaying { get; private set; }

        public MusicPlayer(string location)
        {
            Location = location;
        }

        public void Play()
        {
            IsPlaying = true;
            Console.WriteLine($"Музыка в {Location} включена.");
        }

        public void Stop()
        {
            IsPlaying = false;
            Console.WriteLine($"Музыка в {Location} остановлена.");
        }
    }

    // 3a. Команды света
    public class LightOnCommand : ICommand
    {
        private readonly Light _light;

        public LightOnCommand(Light light) => _light = light;

        public void Execute() => _light.On();
        public void Undo() => _light.Off();
    }

    public class LightOffCommand : ICommand
    {
        private readonly Light _light;

        public LightOffCommand(Light light) => _light = light;

        public void Execute() => _light.Off();
        public void Undo() => _light.On();
    }

    // 3b. Команды кондиционера
    public class AirConditionerOnCommand : ICommand
    {
        private readonly AirConditioner _airConditioner;
        private int _previousTemperature;

        public AirConditionerOnCommand(AirConditioner airConditioner)
        {
            _airConditioner = airConditioner;
        }

        public void Execute()
        {
            _previousTemperature = _airConditioner.Temperature;
            _airConditioner.On();
            _airConditioner.SetTemperature(22);
        }

        public void Undo()
        {
            _airConditioner.SetTemperature(_previousTemperature);
            _airConditioner.Off();
        }
    }

    public class AirConditionerOffCommand : ICommand
    {
        private readonly AirConditioner _airConditioner;

        public AirConditionerOffCommand(AirConditioner airConditioner)
        {
            _airConditioner = airConditioner;
        }

        public void Execute() => _airConditioner.Off();
        public void Undo() => _airConditioner.On();
    }

    // 3c. Команды телевизора
    public class TVOnCommand : ICommand
    {
        private readonly TV _tv;

        public TVOnCommand(TV tv) => _tv = tv;

        public void Execute() => _tv.On();
        public void Undo() => _tv.Off();
    }

    public class TVOffCommand : ICommand
    {
        private readonly TV _tv;

        public TVOffCommand(TV tv) => _tv = tv;

        public void Execute() => _tv.Off();
        public void Undo() => _tv.On();
    }

    // Команды штор
    public class CurtainsOpenCommand : ICommand
    {
        private readonly SmartCurtains _curtains;

        public CurtainsOpenCommand(SmartCurtains curtains) => _curtains = curtains;

        public void Execute() => _curtains.Open();
        public void Undo() => _curtains.Close();
    }

    public class CurtainsCloseCommand : ICommand
    {
        private readonly SmartCurtains _curtains;

        public CurtainsCloseCommand(SmartCurtains curtains) => _curtains = curtains;

        public void Execute() => _curtains.Close();
        public void Undo() => _curtains.Open();
    }

    // Команды плеера
    public class MusicPlayCommand : ICommand
    {
        private readonly MusicPlayer _player;

        public MusicPlayCommand(MusicPlayer player) => _player = player;

        public void Execute() => _player.Play();
        public void Undo() => _player.Stop();
    }

    public class MusicStopCommand : ICommand
    {
        private readonly MusicPlayer _player;

        public MusicStopCommand(MusicPlayer player) => _player = player;

        public void Execute() => _player.Stop();
        public void Undo() => _player.Play();
    }

    // 5. Макрокоманда
    public class MacroCommand : ICommand
    {
        private readonly List<ICommand> _commands;

        public MacroCommand(List<ICommand> commands)
        {
            _commands = commands;
        }

        public void Execute()
        {
            foreach (var command in _commands)
                command.Execute();
        }

        public void Undo()
        {
            for (int i = _commands.Count - 1; i >= 0; i--)
                _commands[i].Undo();
        }
    }

    // 4. Пульт
    public class RemoteControl
    {
        private readonly Dictionary<int, ICommand> _slots = new Dictionary<int, ICommand>();
        private readonly Stack<ICommand> _history = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();

        private bool _isRecording = false;
        private readonly List<ICommand> _recordedCommands = new List<ICommand>();

        public void SetCommand(int slot, ICommand command)
        {
            _slots[slot] = command;
        }

        public void PressButton(int slot)
        {
            ICommand command = _slots.ContainsKey(slot) ? _slots[slot] : new NoCommand();

            command.Execute();

            if (!(command is NoCommand))
            {
                _history.Push(command);
                _redoStack.Clear();

                if (_isRecording)
                    _recordedCommands.Add(command);
            }
        }

        public void UndoButton()
        {
            if (_history.Count > 0)
            {
                ICommand command = _history.Pop();
                command.Undo();
                _redoStack.Push(command);
            }
            else
            {
                Console.WriteLine("История команд пуста.");
            }
        }

        public void RedoButton()
        {
            if (_redoStack.Count > 0)
            {
                ICommand command = _redoStack.Pop();
                command.Execute();
                _history.Push(command);
            }
            else
            {
                Console.WriteLine("Нет команды для повтора.");
            }
        }

        public void StartRecording()
        {
            _isRecording = true;
            _recordedCommands.Clear();
            Console.WriteLine("Запись макрокоманды начата.");
        }

        public MacroCommand StopRecording()
        {
            _isRecording = false;
            Console.WriteLine("Запись макрокоманды завершена.");
            return new MacroCommand(new List<ICommand>(_recordedCommands));
        }
    }
}
