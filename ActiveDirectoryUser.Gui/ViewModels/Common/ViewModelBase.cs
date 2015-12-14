using System.ComponentModel;
using System.Runtime.CompilerServices;
using ActiveDirectoryUser.Gui.Annotations;

namespace ActiveDirectoryUser.Gui.ViewModels.Common
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}