namespace GameStudioClicker.Wpf.ViewModels
{
    public sealed class StatisticViewModel : ViewModelBase
    {
        private readonly Func<long> _valueProvider;
        public string DisplayName { get; }
        public long Value => _valueProvider();

        public StatisticViewModel(string displayName, Func<long> valueProvider)
        {
            if (!string.IsNullOrWhiteSpace(displayName))
            {
                DisplayName = displayName;
            }
            else
            {
                throw new ArgumentException("Display name cannot be null or whitespace.", nameof(displayName));
            }

            _valueProvider = valueProvider ?? throw new ArgumentNullException(nameof(valueProvider));
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(Value));
        }
    }
}
