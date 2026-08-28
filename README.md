# Game Studio Clicker

Game Studio Clicker is a small C# and WPF clicker game built as a learning project. Write lines of code, spend them on mechanical keyboards, and increase the number of lines produced by each click.

## Current features

- Write code manually, starting at one line per click.
- Buy mechanical keyboards with accumulated lines of code.
- Gain one additional line per click for every keyboard owned.
- Buy multiple keyboards; the price doubles after each purchase.
- Disable purchases automatically when the player cannot afford them.
- Keep game rules separate from the WPF interface with an MVVM-style design.
- Cover the game state and view-model behavior with MSTest tests.

The project is currently an early playable prototype. Progress exists only for the current session; save/load and offline or automatic production have not been implemented yet.

## Requirements

- Windows
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Run the game

From the repository root:

```powershell
dotnet run --project src/GameStudioClicker.Wpf
```

## Run the tests

```powershell
dotnet test GameStudioClicker.sln
```

## Solution structure

- `src/GameStudioClicker.Core` contains platform-independent game state and rules.
- `src/GameStudioClicker.Wpf` contains the Windows UI, commands, and view models.
- `tests/GameStudioClicker.Tests` contains MSTest coverage for the Core and ViewModel layers.

## Current gameplay balance

- A new game starts with 0 lines of code and 1 line per click.
- The first mechanical keyboard costs 10 lines of code.
- Each keyboard adds 1 line per click.
- Each purchase doubles the cost of the next keyboard.
