using System.ComponentModel;
using GameStudioClicker.Wpf.ViewModels;

namespace GameStudioClicker.Tests;

[TestClass]
public class ViewModelBaseTests
{
    private sealed class TestViewModel : ViewModelBase
    {
        public void NotifyPropertyChanged(string? propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }
    }

    [TestMethod]
    public void OnPropertyChanged_RaisesPropertyChangedWithName()
    {
        // Arrange
        var viewModel = new TestViewModel();
        PropertyChangedEventArgs? eventArgs = null;
        viewModel.PropertyChanged += (_, args) => eventArgs = args;

        // Act
        viewModel.NotifyPropertyChanged("ExampleProperty");

        // Assert
        Assert.IsNotNull(eventArgs);
        Assert.AreEqual("ExampleProperty", eventArgs.PropertyName);
    }
}
