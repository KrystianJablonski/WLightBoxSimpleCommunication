using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.MVVM
{
    public class ViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private bool _canExecute;
        public bool CanExecuteValue
        {
            get => _canExecute;
            set
            {
                _canExecute = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private Action _executeAction;

        public ViewModelCommand(Action executeAction, bool canExecute = true)
        {
            if (executeAction == null)
                throw new ArgumentNullException(nameof(executeAction));
            _executeAction = executeAction;
            canExecute = true;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _executeAction.Invoke();
        }
    }
}
