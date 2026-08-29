# Game Studio Clicker

Game Studio Clicker is a small C# and WPF idle/clicker game built as a learning project. Write lines of code, improve active production with mechanical keyboards, and hire interns to generate code automatically.

## Current features

- Write code manually, starting at one line per click.
- Buy mechanical keyboards with accumulated lines of code.
- Gain one additional line per click for every keyboard owned.
- Buy multiple keyboards; the price doubles after each purchase.
- Hire interns that generate lines of code automatically every second.
- Gain two additional lines per second for every intern hired.
- Buy multiple interns; the hiring cost doubles each time.
- Disable purchases automatically when the player cannot afford them.
- Display active and passive production separately.
- Use a two-panel dark interface with reusable button styling and scrollable upgrade cards.
- Keep game rules separate from the WPF interface with an MVVM-style design.
- Cover the game state and view-model behavior with MSTest tests.

The project is currently an early playable prototype. Progress exists only for the current session; save/load and offline progression have not been implemented yet.

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
- Passive production starts at 0 lines per second.
- The first mechanical keyboard costs 25 lines of code.
- Each keyboard adds 1 line per click.
- The first intern costs 50 lines of code.
- Each intern adds 2 lines per second.
- Each upgrade type doubles in cost after every purchase.
