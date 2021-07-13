using System.Windows.Input;
using Microsoft.Win32;
using WpfCatalogReader.Moduls.Components;

namespace WpfCatalogReader
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ICommand _openFileCommand;

        public ICommand OpenFileCommand
            => _openFileCommand ??= new RelayCommand(c => Open());

        public MainWindowViewModel()
        {
            
        }
        
        private void Open()
        {
            var filterName = Settings.Default.IsEnglish
                ? "Dictionary|*.po|All files|*.*"
                : "Словарь|*.po|Все файлы|*.*";

            var dlg = new OpenFileDialog
            {
                DefaultExt = "po",
                Filter = filterName
            };

            if (dlg.ShowDialog() != true ||
                string.IsNullOrEmpty(dlg.FileName))
                return;
            
            Settings.Default.OpenFile(dlg.FileName);
        }
    }
}