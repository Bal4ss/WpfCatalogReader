using System.Configuration;
using WpfCatalogReader.Moduls.Convertor;

namespace WpfCatalogReader.Moduls.Config
{
    public class AppSettings
    {
        private const string IsEnglishAttribute = "isEnglish";
        public AppSettings() { }

        public bool IsEnglish
        {
            get => CustConvertor.GetBoolFromString(ConfigurationManager.AppSettings[IsEnglishAttribute]);
            set => ConfigurationManager.AppSettings[IsEnglishAttribute] = value.ToString();
        }
    }
}