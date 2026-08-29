using GameStudioClicker.Wpf.Commands;

namespace GameStudioClicker.Tests;

[TestClass]
public class RelayCommandTests
{
    [TestMethod]
    public void Constructor_WithNullExecute_ThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new RelayCommand(null!));
    }

    [TestMethod]
    public void CanExecute_WithoutPredicate_ReturnsTrue()
    {
        var command = new RelayCommand(_ => { });

        Assert.IsTrue(command.CanExecute(null));
    }

    [TestMethod]
    public void CanExecute_WithPredicate_ReturnsPredicateResult()
    {
        var command = new RelayCommand(_ => { }, parameter => (bool)parameter!);

        Assert.IsTrue(command.CanExecute(true));
        Assert.IsFalse(command.CanExecute(false));
    }

    [TestMethod]
    public void Execute_InvokesActionWithParameter()
    {
        object? receivedParameter = null;
        var command = new RelayCommand(parameter => receivedParameter = parameter);

        command.Execute("value");

        Assert.AreEqual("value", receivedParameter);
    }

    [TestMethod]
    public void RaiseCanExecuteChanged_RaisesCanExecuteChangedEvent()
    {
        var command = new RelayCommand(_ => { });
        bool eventRaised = false;
        command.CanExecuteChanged += (_, _) => eventRaised = true;

        command.RaiseCanExecuteChanged();

        Assert.IsTrue(eventRaised);
    }
}
