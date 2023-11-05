using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Grant2FW.AdditionalClasses
{
    internal class ClassChangeBackgroundColorCommand : ICommand
    {
        private Action execute;

        public ClassChangeBackgroundColorCommand(Action execute)
        {
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke();
        }
    }
}
