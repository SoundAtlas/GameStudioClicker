# Game Studio Clicker

Game Studio Clicker is a playable C# and WPF idle/clicker prototype built as a
learning project. The player writes Lines of Code manually, purchases one-time
upgrades, and hires employees who produce code automatically.

The project focuses on readable C#, practical MVVM, JSON persistence, incremental
refactoring, and learning how game rules connect to a responsive desktop interface.

## Current features

### Production and progression

- Write one Line of Code per click at the start of a new game.
- Purchase chained active upgrades that multiply click production.
- Purchase targeted upgrades for Interns, Junior Developers, Senior Developers,
  and Lead Developers.
- Purchase a late-game global upgrade that affects every worker type.
- Require ownership of the targeted worker type before its production upgrades
  can be purchased.
- Preview upcoming active upgrades and explain unmet requirements in tooltips.
- Hire repeatable workers whose costs double after every purchase.
- Unlock later worker types through ownership requirements.
- Show the next worker as a mystery card with requirement progress, keep its
  identity hidden when it becomes hireable, and reveal it after the first hire.

| Worker | Base cost | Base production | Unlock requirement |
|---|---:|---:|---|
| Intern | 50 | 2/second | None |
| Junior Developer | 2,000 | 20/second | Own 5 Interns |
| Senior Developer | 20,000 | 2,000/second | Own 5 Junior Developers |
| Lead Developer | 200,000 | 20,000/second | Own 1 Senior Developer |

All economy values are provisional. Current playtest observations are recorded in
[`BALANCING_NOTES.md`](BALANCING_NOTES.md).

### Saving and offline progress

- Load progress automatically on startup.
- Save every 30 seconds, after purchases, and when the window closes.
- Persist Lines of Code, purchased active-upgrade IDs, worker counts, and the last
  save time as readable JSON.
- Award up to 24 hours of passive production while the game is closed.
- Preserve malformed saves with a timestamped `.corrupt-*` suffix and start safely.
- Tolerate missing save collections and clamp negative persisted values.

On Windows, the save file is stored at:

```text
%LocalAppData%\GameStudioClicker\game_save.json
```

### Lifetime statistics

- Track Lifetime Lines of Code, manual clicks, employees hired, and active
  upgrades purchased.
- Split generated Lines of Code by source (manual or workers) and play state
  (online or offline).
- Persist statistics with the rest of the save data and restore them on startup.
- Present statistics through reusable ViewModels and an `ItemsControl` so new
  rows do not require duplicated XAML.

### Interface and feedback

- Use a modern dark pixel interface built around deep navy surfaces, crisp pixel
  artwork, and restrained cyan, purple, pink, and green accents.
- Display bundled artwork for the game logo, Lines of Code currency, active
  upgrades, and worker portraits.
- Bundle Inter for readable interface text and Press Start 2P for selected section
  headings without requiring fonts to be installed on the player's computer.
- Use a three-part dashboard for branding, manual production, and active upgrades.
- Distinguish immediately purchasable active upgrades with a persistent cyan
  outline while dimming upgrades that cannot currently be bought.
- Show workers in a compact, stable three-column roster with the whole card acting
  as the hire action.
- Keep worker cards focused on identity, owned count, and hire cost while moving
  detailed production information into consistent tooltips.
- Keep scrolling limited to the worker progression area.
- Show concurrent floating click values that alternate left and right.
- Pulse newly affordable active upgrades.
- Animate worker hover, press, and purchase feedback using independent visual
  layers so repeated affordable purchases remain visible.
- Animate offline earnings without shifting the dashboard.
- Keep theme resources, control styles, and reusable Storyboards in separate WPF
  resource dictionaries.

The current visual reference and asset guidance live in
[`Design/style/`](Design/style/), while game-ready artwork and bundled fonts live
under [`src/GameStudioClicker.Wpf/Assets/`](src/GameStudioClicker.Wpf/Assets/).

## Architecture

- `GameStudioClicker.Core` contains platform-independent models, game rules, and
  persistence data.
- `GameStudioClicker.Wpf` contains the Windows interface, ViewModels, commands,
  formatting, timers, and window lifecycle integration.
- `GameStudioClicker.Tests` contains focused MSTest coverage for game rules,
  ViewModels, commands, and JSON persistence.
- `Styles/Theme.xaml`, `Styles/Animations.xaml`, and `Styles/Controls.xaml` separate
  shared visual resources from individual views.
- `ActiveUpgradesView`, `WorkerUpgradesView`, and `StatisticsView` own their
  respective interface sections, leaving `MainWindow` responsible for the overall
  shell and navigation.

The Model owns economy rules and derived production. ViewModels adapt that state for
binding and commands. The View owns layout and purely visual animation behavior.

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

## Next milestone

The next short-term milestone is the first small set of achievements, built on the
lifetime statistics and progression state now available. Responsive and high-DPI
visual checks and executable-icon configuration remain small interface follow-ups.
Gameplay balancing is still intentionally deferred and tracked separately in
`BALANCING_NOTES.md`.

See [`ROADMAP.md`](ROADMAP.md) for the complete milestone history and planned work.
