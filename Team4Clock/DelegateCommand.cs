using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Team4Clock
{
    /// <summary>
    /// Simple implentation of a delegate command.
    /// 
    /// This is basically a convenience class so that Commands don't have to be 
    /// written manually.
    /// </summary>
    class DelegateCommand : ICommand
    {
        private readonly Action _action;

        /// <summary>
        /// DelegateCommand constructor. Takes an action to delegate as a command.
        /// </summary>
        /// <param name="action"></param>
        public DelegateCommand(Action action)
        {
            _action = action;
        }

        // Code below should not be invoked manually (XAML-handled).

        public void Execute(object param)
        {
            _action();
        }

        public bool CanExecute(object param)
        {
            return true;
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67
    }
}
