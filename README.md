# Game Studio Clicker

Game Studio Clicker is a small C# and WPF idle/clicker game built as a learning project. The player writes Lines of Code manually, buys one-time hardware upgrades, and hires repeatable workers for passive production.

The project is an early playable prototype focused on learning C#, WPF, MVVM, persistence, and gradual refactoring.

## Current features

- Write code manually, starting at one line per click.
- Purchase one-time hardware upgrades that multiply click production.
- Display the first two unpurchased active upgrades while hiding upgrades further ahead.
- Disable active upgrades when they are unaffordable or their prerequisite has not been purchased.
- Remove active upgrades from the store after purchase.
- Hire repeatable workers that generate Lines of Code every second.
- Increase a worker's cost after each purchase.
- Unlock later worker types by owning enough of the previous worker.
- Show the next locked worker as a `???` preview with its cost, unlock requirement, and progress.
- Keep worker-card sizes consistent when changing between mystery and unlocked states.
- Disable purchases automatically when the player cannot afford them.
- Display click and passive production separately.
- Use a two-panel dark interface with reusable button and tooltip styling.
- Save progress automatically as readable JSON when the game closes.
- Load saved progress automatically when the game starts.
- Persist active-upgrade IDs and worker counts through generic collections.
- Award up to 24 hours of passive production while the game is closed.
- Show a temporary notification summarizing offline earnings.
- Keep game rules separate from the WPF interface with an MVVM-style design.

## Current progression

### Active upgrades

Active upgrades are strong, one-time purchases. The current hardware progression is:

| Upgrade | Cost | Effect |
|---|---:|---:|
| Mouse Pad | 100 | Click production x2 |
| Gaming Mouse | 400 | Click production x2 |
| Mechanical Keyboard | 700 | Click production x2 |
| Headset | 800 | Click production x2 |
| Webcam | 900 | Click production x2 |
| External SSD | 1,000 | Click production x2 |
| Second Monitor | 1,500 | Click production x2 |
| Ultrawide Monitor | 3,000 | Click production x3 |

The current upgrades form a prerequisite chain. The UI previews two unpurchased upgrades at a time, with unavailable upgrades shown disabled.

### Workers

Workers are repeatable purchases. Their current cost doubles after every hire.

| Worker | Base cost | Production | Unlock requirement |
|---|---:|---:|---|
| Intern | 50 | 2 lines/second | None |
| Junior Developer | 2,000 | 20 lines/second | Own 5 Interns |
| Senior Developer | 20,000 | 2,000 lines/second | Own 5 Junior Developers |

These values are provisional and still need balancing.

## Save data

The game saves automatically when the main window closes and loads automatically on startup. On Windows, the save file is stored at:

```text
%LocalAppData%\GameStudioClicker\game_save.json
```

The save contains the current Lines of Code, purchased active-upgrade IDs, worker counts, and a UTC timestamp used to calculate offline progress. Offline production is limited to 24 hours per session away.

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

The test project covers Core rules, commands, ViewModels, and JSON persistence. Some tests still need to be updated after the latest active-upgrade and worker refactors.

## Solution structure

- `src/GameStudioClicker.Core` contains platform-independent game state and rules.
- `src/GameStudioClicker.Core/Models/ActiveUpgrade.cs` represents one-time active upgrades.
- `src/GameStudioClicker.Core/Models/WorkerUpgrade.cs` represents repeatable passive workers.
- `src/GameStudioClicker.Core/Persistence` contains save-data and JSON persistence classes.
- `src/GameStudioClicker.Wpf` contains the Windows UI, commands, timers, and ViewModels.
- `tests/GameStudioClicker.Tests` contains MSTest coverage.

## Planned improvements

- Rebalance active-upgrade prices, click multipliers, worker prices, and worker production.
- Allow active upgrades to affect systems other than click production, such as Intern productivity.
- Improve the visual distinction between an unaffordable upgrade and one blocked by a prerequisite.
- Add proper artwork or icons for active upgrades and workers.
- Finish removing legacy worker save fields and update tests for the generic worker system.
- Continue improving saving, code structure, and general polish in later milestones.
