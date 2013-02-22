using System;
using System.Windows.Input;

namespace TeamCityDesktop.ViewModel
{
    public class CommandViewModel : ViewModelBase
    {
        public CommandViewModel(string commandName, ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            CommandName = commandName;
            Command = command;
        }

        public string CommandName { get; private set; }

        public ICommand Command { get; private set; }
    }
}
