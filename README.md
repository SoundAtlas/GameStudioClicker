# Game Studio Clicker

Game Studio Clicker is a playable C# and WPF idle/clicker prototype built as a
learning project. The player writes Lines of Code manually, purchases one-time
upgrades, hires employees who produce code automatically, earns achievements,
and gains Studio XP through active gameplay.

The project focuses on readable C#, practical MVVM, JSON persistence, incremental
refactoring, and learning how game rules connect to a responsive desktop interface.

## Current features

### Production and progression

- Write Lines of Code manually, with an intended starting click power of one and
  a temporary development override described below.
- Purchase chained active upgrades that multiply click production.
- Purchase targeted upgrades for six worker tiers, from Intern through Studio
  Director.
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
| Engineering Manager | 2,000,000 | 200,000/second | Own 1 Lead Developer |
| Studio Director | 20,000,000 | 2,000,000/second | Own 1 Engineering Manager |

All economy values are provisional. Current playtest observations are recorded in
[`BALANCING_NOTES.md`](BALANCING_NOTES.md).

The starting click power is temporarily elevated during development so new content
can be reached quickly while testing. It is not a final balance value.

### Studio Level and experience

- Start at Studio Level 1 and derive the level from cumulative Studio XP.
- Reach Levels 2–5 at 100, 250, 500, and 900 total XP respectively.
- Award 1 XP per manual coding action, regardless of Lines per Click.
- Award 10 / 25 / 50 / 100 / 200 / 400 XP per successful hire, from Intern through
  Studio Director.
- Award 25 / 50 / 100 / 150 XP per successful active-upgrade purchase in content
  Eras 1–4.
- Award a one-time XP bonus for each newly earned achievement, as listed below.
- Award no direct XP for passive or offline production, failed purchases, or
  navigation. A newly earned achievement can still grant its one-time bonus.
- Display Studio Level, current-level XP progress, and a rounded progress bar in
  the dashboard; show a full bar and `MAX LEVEL` at Level 5.
- Retain total XP beyond Level 5 and show it as Lifetime Experience in Statistics.
- Persist total XP and derive the level when loading; restoring existing purchases
  and earned achievements does not repeat their rewards.

Locations, relocation, and location-based worker gates are planned but not yet
implemented. Current worker unlocks still use employee ownership requirements.

### Saving and offline progress

- Load progress automatically on startup.
- Save every 30 seconds, after purchases, and when the window closes.
- Persist Lines of Code, Studio XP, lifetime statistics, purchased active-upgrade
  IDs, worker counts, earned achievement IDs, and the last save time as readable JSON.
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
- Display cumulative Studio XP as Lifetime Experience, using the existing
  progression value rather than a separate counter.
- Split generated Lines of Code by source (manual or workers) and play state
  (online or offline).
- Persist statistics with the rest of the save data and restore them on startup.
- Present statistics through reusable ViewModels and an `ItemsControl` so new
  rows do not require duplicated XAML.

### Achievements

- Track five achievements with one-time Studio XP rewards:

  | Achievement | Requirement | XP reward |
  |---|---|---:|
  | Hello World | First manual click | 5 |
  | First Hire | First employee hired | 10 |
  | First Upgrade | First active upgrade purchased | 10 |
  | Shipping Code | 1,000 lifetime Lines of Code | 25 |
  | Hello, Game Dev | Studio Level 5 | 50 |

- Calculate progress from existing lifetime statistics and progression state.
- Persist earned achievement IDs and restore them without awarding duplicates.
- Present achievements as a wrapping grid of progress tiles on a dedicated
  page separate from lifetime statistics.
- Queue achievement unlock notifications so closely spaced unlocks are shown in
  order.
- Display animated, Steam-style achievement notifications without moving or
  blocking the game interface.

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
- Show six workers in a compact two-row, three-column roster with the whole card
  acting as the hire action.
- Keep worker cards focused on identity, owned count, and hire cost while moving
  detailed production information into consistent tooltips.
- Keep the worker roster independently scrollable and give statistics,
  achievements, and settings their own ScrollViewers.
- Share rounded progress-bar styling between Studio XP and achievements, retaining
  the green earned-achievement appearance.
- Use themed volume sliders that support clicking the track and immediately
  dragging, plus shared slim vertical scrollbars.
- Show concurrent floating click values that alternate left and right.
- Pulse newly affordable active upgrades.
- Animate worker hover, press, and purchase feedback using independent visual
  layers so repeated affordable purchases remain visible.
- Animate offline earnings without shifting the dashboard.
- Play five shuffled soundtrack tracks without repeating a track until the shuffle
  is refilled, and display the current track title.
- Provide sound effects for coding, navigation, purchases, achievement unlocks,
  and enabled-button hover; reuse each cue's player for rapid repetitions.
- Provide separate music and sound-effect volume controls on a settings page,
  with mute at zero and smooth real-time soundtrack volume changes.
- Persist audio percentages immediately in
  `%LocalAppData%\GameStudioClicker\settings.json`, separately from game progress,
  and restore them before soundtrack playback.
- Navigate independently between the worker roster, lifetime statistics,
  achievements, and settings.
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
- `IGameSaveRepository` separates game-session logic from the current
  `JsonGameSaveRepository`, leaving room for another persistence implementation
  later.
- `Styles/Theme.xaml`, `Styles/Animations.xaml`, and `Styles/Controls.xaml` separate
  shared visual resources from individual views.
- `ActiveUpgradesView`, `WorkerUpgradesView`, `StatisticsView`, `AchievementsView`,
  and `SettingsView` own their respective interface sections, leaving `MainWindow`
  responsible for the overall shell, page navigation, and global notifications.
- `AudioService` owns soundtrack and sound-effect playback, while
  `SmoothedVolumeSampleProvider` prevents noise during live music-volume changes.
- `StudioProgression` owns XP and level calculations; `GameState` awards action
  rewards and evaluates achievements, with content values in `GameContentFactory`.
- `StatisticViewModel` provides reusable statistic rows whose values are read from
  the current game state and refreshed through property-change notifications.
- `IApplicationSettingsRepository` and `JsonApplicationSettingsRepository` keep
  audio settings separate from the core game save.
- `ClickDragSlider` owns the volume sliders' track-click and drag interaction.

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

Continue Studio progression with Phase 7: a location model, followed by relocation
rules. The first planned locations are Bedroom/Shed and Small Office, with Small
Office proposed at Level 5 for 5,000 Lines of Code. Location persistence and
existing-save compatibility come before changing worker gates.

Core leveling, the planned action XP sources, XP persistence, and the dashboard
display are implemented. Focused leveling, reward, achievement, and persistence
regression tests and specific manual edge-case checks remain follow-ups.

Keep the temporary click-power override for development and separate it from the
intended balance before release. Responsive, minimum-window-size, and high-DPI
layout work is deferred until the progression panels settle. Executable-icon
configuration, audio attribution, and audio stress checks remain pending. Gameplay
balancing is intentionally deferred and tracked in `BALANCING_NOTES.md`.

See [`ROADMAP.md`](ROADMAP.md) for the complete milestone history and planned work.
