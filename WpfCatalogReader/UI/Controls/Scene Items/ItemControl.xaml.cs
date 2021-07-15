using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfCatalogReader.Moduls.Model;

namespace WpfCatalogReader.UI.Controls.Scene_Items
{
    public partial class ItemControl : UserControl
    {
        public ItemControl(List<ItemModel> items)
        {
            InitializeComponent();

            ViewModel = new ItemControlVm(items)
            {
                ItemList = ItemListPanel.Children,
            };
        }

        public ItemControlVm ViewModel
        {
            get => (ItemControlVm) DataContext;
            private set => DataContext = value;
        }
    }
}