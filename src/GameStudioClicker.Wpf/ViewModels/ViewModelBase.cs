using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameStudioClicker.Wpf.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        // Shared notification plumbing for properties exposed by all view models.
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
