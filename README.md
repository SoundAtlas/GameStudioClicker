# Game Studio Clicker

A small C# and WPF idle/clicker game built as a learning project.

## Solution structure

- `src/GameStudioClicker.Wpf` contains the WPF user interface and MVVM presentation layer.
  - `Views` contains windows and user controls.
  - `ViewModels` contains presentation state and UI behavior.
  - `Commands` contains `ICommand` implementations.
  - `Converters` contains WPF value converters when they become necessary.
- `src/GameStudioClicker.Core` contains game rules and data that do not depend on WPF.
  - `Models` contains game state and domain objects.
  - `Services` contains game operations that deserve their own responsibility.
  - `Persistence` will contain save/load-related abstractions and implementations.
- `tests/GameStudioClicker.Tests` contains MSTest tests for the Core and ViewModel layers.

The initial scaffold intentionally contains no game implementation.
