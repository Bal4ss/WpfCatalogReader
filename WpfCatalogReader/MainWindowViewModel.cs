using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using WpfCatalogReader.Moduls.Components;
using WpfCatalogReader.Moduls.Model;
using WpfCatalogReader.UI.Controls.Scene_Items;

namespace WpfCatalogReader
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ICommand _openFileCommand;
        private UIElementCollection _uiIds;
        private UIElementCollection _uiItems;

        public ICommand OpenFileCommand
            => _openFileCommand ??= new RelayCommand(c => Open());

        public UIElementCollection UiItems
        {
            get => _uiItems;
            set
            {
                _uiItems = value;
                RaisePropertyChanged(nameof(UiItems));
            }
        }

        public UIElementCollection UiIds
        {
            get => _uiIds;
            set
            {
                _uiIds = value;
                RaisePropertyChanged(nameof(UiIds));
            }
        }
        
        public MainWindowViewModel()
        {
            Settings.Default.Select += (sender, args) => Select();
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

            Reload();
        }

        private void Select()
        {
            UiIds?.Clear();
            var items = Settings.Default.SelectedItems?.Select(c => c.ItemId).ToList();
            
            items?.ForEach(c =>UiIds?.Add(new TextBlock()
                {
                    Text = c,
                    Margin = new Thickness(2.5)
                }));
        }
        
        private void Reload()
        {
            UiItems?.Clear();
            
            var defaultItemList = Settings.Default.Items;
                
            var tmpList = new List<ItemModel>();
            foreach (var a in defaultItemList)
            {
                tmpList.Add(a);
                var itemsPath = GetNewContextPath(a.ItemContext);
                while (!string.IsNullOrEmpty(itemsPath))
                {
                    tmpList.Add(new ItemModel() { ItemContext = itemsPath });
                    itemsPath = GetNewContextPath(itemsPath);
                }
            }
            
            var itemList = tmpList.GroupBy(c => c.ItemContext)
                .OrderBy(c => c.Key)
                .Select(c => new ItemControl(c.Select(b => b).ToList()))
                .ToList();
            
            for (var i = 0; i < itemList.Count(); i++)
            {
                var tmpCts = itemList[i].ViewModel.ContextGroup;
                for (var j = i + 1; j < itemList.Count(); j++)
                {
                    var tmpCtsJ = itemList[j].ViewModel.ContextPath;
                    if (tmpCts == tmpCtsJ)
                        itemList[i].ViewModel.AddItem(itemList[j]);
                }
            }

            var result = itemList.FirstOrDefault(c => c.ViewModel.ContextDirectory == "Root");
            if (result != null)
            {
                result.ViewModel.IsRoot = true;
                UiItems?.Add(result);
            }
            RaisePropertyChanged(nameof(UiItems));
        }

        private string GetNewContextPath(string str)
        {
            var tmpList = str?.Split('.').ToList();
            return string.Join(
                ".",
                tmpList?.Where((c, i) => i != tmpList?.Count() -1).ToList()
                ?? new List<string>());;
        }
    }
}