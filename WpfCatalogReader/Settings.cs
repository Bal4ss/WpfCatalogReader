using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Karambolo.PO;
using WpfCatalogReader.Moduls.Config;
using WpfCatalogReader.Moduls.Model;

namespace WpfCatalogReader
{
    public class Settings
    {
        private readonly AppSettings _appSettings;

        private List<ItemModel> _items = new List<ItemModel>();
        private List<ItemModel> _selectedItems;
        private event EventHandler _select;
        
        public static readonly Settings Default = new Settings();

        public Settings()
        {
            _appSettings = new AppSettings();
        }

        public bool IsEnglish => _appSettings.IsEnglish;

        public List<ItemModel> Items => _items;

        public EventHandler Select
        {
            get => _select;
            set => _select = value;
        }

        public List<ItemModel> SelectedItems
            => _selectedItems ??= new List<ItemModel>();

        public void SelectItem(List<ItemModel> models)
        {
            _selectedItems = models;
            Select?.Invoke(this, EventArgs.Empty);
        }
        
        public void OpenFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(filename);

            var parser = new POParser();

            POParseResult result;
            using (var reader = new StreamReader(filename, Encoding.UTF8))
                result = parser.Parse(reader);

            if (!result.Success) return;
            var catalog = result.Catalog;
            
            foreach (var a in catalog?.Keys ?? new List<POKey>())
            {
                _items.Add(new ItemModel()
                {
                    ItemContext = a.ContextId,
                    ItemId = a.Id
                });
            }
        }
    }
}