using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfCatalogReader.Annotations;
using WpfCatalogReader.Moduls.Components;
using WpfCatalogReader.Moduls.Model;

namespace WpfCatalogReader.UI.Controls.Scene_Items
{
    public partial class ItemControl : UserControl, INotifyPropertyChanged
    {
        private ICommand _activeCommand;
        private ICommand _selectCommand;

        private List<ItemControl> _itemsControlList = new List<ItemControl>();
        private List<ItemModel> _items;
        private bool _isActive;
        private bool _isRoot;

        public ICommand ActiveCommand
            => _activeCommand ??= new RelayCommand(c => Show());
        public ICommand SelectCommand
            => _selectCommand ??= new RelayCommand(c => Select());
        
        public ItemControl(List<ItemModel> items)
        {
            InitializeComponent();
            
            _items = items;
            IsActive = true;
            IsRoot = false;
            
            Reload();
        }

        public bool IsActive
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
                Reload();
            }
        }

        public string ActiveButtonText => _isActive ? "-" : "+";

        public string ContextDirectory
            => _items.FirstOrDefault()?.ItemContext?.Split('.').LastOrDefault()
                ?? "Root";

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
                    tmpList?.Where((c, i) => i != tmpList?.Count() -1).ToList()
                    ?? new List<string>());;
            }
        }
        
        public string ContextGroup
            => _items.FirstOrDefault()?.ItemContext ?? "";
        
        public Visibility IsShowing 
            => _isActive && _itemsControlList.Any() ? Visibility.Visible : Visibility.Collapsed;

        public void AddItem(ItemControl newItem)
        {
            _itemsControlList?.Add(newItem);
            Reload();
        }
        
        private void Show()
        {
            IsActive = !IsActive;
            ItemListPanel.Visibility = IsShowing;
            ShowHideSideButton.Visibility = IsShowing;
        }

        private void Reload()
        {
            ItemListPanel.Visibility = IsShowing;
            DirectoryName.Text = ContextDirectory;
            ShowHideSideButton.Visibility = IsShowing;
            SideBorderBlock.Visibility = IsRoot ? Visibility.Collapsed : Visibility.Visible;
            ShowHideButton.Visibility = _itemsControlList.Any() ? Visibility.Visible : Visibility.Collapsed;
            
            SideBorderBlock.Margin = new Thickness((_itemsControlList.Any() ? -40 : -30), 0, 0, 0);
            
            ItemListPanel.Children.Clear();
            _itemsControlList?.ForEach(c => ItemListPanel.Children.Add(c));
        }

        private void Select()
        {
            Settings.Default.SelectItem(_items);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}