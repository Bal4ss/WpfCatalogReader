using System;
using System.ComponentModel;

namespace WpfCatalogReader.Moduls.Components
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (propertyName == null) 
                throw new ArgumentNullException(nameof(propertyName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}