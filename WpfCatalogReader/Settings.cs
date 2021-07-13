using System;
using System.Collections.Generic;
using WpfCatalogReader.Moduls.Config;
using WpfCatalogReader.Moduls.Data_Provider;
using WpfCatalogReader.Moduls.Model;

namespace WpfCatalogReader
{
    public class Settings
    {
        private readonly AppSettings _appSettings;

        private List<ItemModel> _items = new List<ItemModel>();
        
        public static readonly Settings Default = new Settings();

        public Settings()
        {
            _appSettings = new AppSettings();
        }

        public bool IsEnglish => _appSettings.IsEnglish;

        public List<ItemModel> Items => _items;
        
        public void OpenFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(filename);

            var list = FileDataProvider.GetData(filename);
        }
    }
}