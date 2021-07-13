namespace WpfCatalogReader.Moduls.Convertor
{
    public class CustConvertor
    {
        public static bool GetBoolFromString(string str)
            => !string.IsNullOrEmpty(str) && bool.TryParse(str, out var result) && result;
    }
}