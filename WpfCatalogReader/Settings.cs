using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private event EventHandler _reload;
        
        public static readonly Settings Default = new Settings();

        public Settings()
        {
            _appSettings = new AppSettings();
        }

        public bool IsEnglish => _appSettings.IsEnglish;

        public List<ItemModel> Items => _items;

        public EventHandler Reload
        {
            get => _reload;
            set => _reload = value;
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
                    ItemContext = a.ContextId?.Split('.').ToList(),
                    ItemId = a.Id
                });
            }

            Reload?.Invoke(this, EventArgs.Empty);
        }
    }
}