using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfCatalogReader.Moduls.Components;
using WpfCatalogReader.Moduls.Model;

namespace WpfCatalogReader.UI.Controls.Scene_Items
{
    public class ItemControlVm : ViewModelBase
    {
        private ICommand _activeCommand;
        private ICommand _selectCommand;

        private List<ItemControl> _itemsControlList = new List<ItemControl>();
        private List<ItemModel> _items;
        private bool _isActive;
        private bool _isRoot;

        private UIElementCollection _itemList;

        public ICommand ActiveCommand
            => _activeCommand ??= new RelayCommand(c => Show());
        public ICommand SelectCommand
            => _selectCommand ??= new RelayCommand(c => Select());
        
        public ItemControlVm(List<ItemModel> items)
        {
            _items = items;
            IsActive = true;
            IsRoot = false;
        }

        public UIElementCollection ItemList
        {
            get => _itemList;
            set => _itemList = value;
        }

        private bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                RaisePropertyChanged(nameof(IsShowing));
                RaisePropertyChanged(nameof(ActiveButtonText));
            }
        }
        
        public bool IsRoot
        {
            get => _isRoot;
            set
            {
                _isRoot = value;
                RaisePropertyChanged(nameof(IsShowingRoot));
            }
        }

        public string ActiveButtonText => _isActive ? "-" : "+";

        public string ContextDirectory
        {
            get => _items.FirstOrDefault()?.ItemContext?.Split('.').LastOrDefault() ?? "Root";
            set => throw new Exception("An attempt ot modify Read-Only property");
        }

        public string ContextPath
        {
            get
            {
                var tmpList = _items.FirstOrDefault()?
                    .ItemContext?
                    .Split('.')
                    .ToList();
                
                return string.Join(
                    ".",
                    tmpList?.Where((c, i) => i != tmpList.Count() - 1).ToList()
                    ?? new List<string>());
            }
        }
        
        public string ContextGroup
            => _items.FirstOrDefault()?.ItemContext ?? "";
        
        public Visibility IsShowing 
            => _isActive && _itemsControlList.Any() ? Visibility.Visible : Visibility.Collapsed;
        
        public Visibility IsShowingBtn
            => _itemsControlList.Any() ? Visibility.Visible : Visibility.Collapsed;
        
        public Visibility IsShowingRoot
            => IsRoot ? Visibility.Collapsed : Visibility.Visible;
        
        public Thickness SideBorderMargin
            => new Thickness((_itemsControlList.Any() ? -40 : -30), 0, 0, 0);
        
        public void AddItem(ItemControl newItem)
        {
            _itemsControlList?.Add(newItem);
            ItemList?.Clear();
            _itemsControlList?.ForEach(c => ItemList?.Add(c));
            
            RaisePropertyChanged(nameof(IsShowing));
            RaisePropertyChanged(nameof(IsShowingBtn));
            RaisePropertyChanged(nameof(ContextDirectory));
            RaisePropertyChanged(nameof(SideBorderMargin));
        }
        
        private void Show()
        {
            IsActive = !IsActive;
            RaisePropertyChanged(nameof(IsShowing));
        }

        private void Select()
        {
            Settings.Default.SelectItem(_items);
        }
    }
}