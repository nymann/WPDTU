using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UMLaut.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

}