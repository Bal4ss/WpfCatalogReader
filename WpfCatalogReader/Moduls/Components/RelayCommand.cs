using System;
using System.Windows.Input;

namespace WpfCatalogReader.Moduls.Components
{
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        
        public RelayCommand(Action<object> execute) : this(null, execute) { }
        
        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }
        
        public void Execute(object parameter)
            => _execute?.Invoke(parameter);
        
        public bool CanExecute(object parameter)
            => _canExecute == null || _canExecute(parameter);
        
        public void RefreshCanExecute()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        
        public event EventHandler CanExecuteChanged;
    }
}