# Game Studio Clicker Roadmap

This file tracks completed milestones and the next planned areas of development.
Each milestone should be broken into learning-sized tasks before implementation.

## Development notes

- Balance values remain provisional until the dedicated balancing milestone.
- Keep new systems simple and expand the architecture gradually.
- Add tests selectively for important game rules and regressions.
- The user does not want unit-test writing assigned to them.

## Completed milestones

### 1. Manual code production

- Generate Lines of Code by clicking.
- Display current Lines of Code and production values.
- Keep game rules separate from the WPF interface.

### 2. Active upgrades

- Purchase one-time upgrades that improve click or worker production.
- Support upgrade prerequisites, affordability checks, and ownership requirements.
- Hide purchased upgrades and preview upcoming upgrades.
- Explain unmet requirements in tooltips.

### 3. Passive production and workers

- Hire repeatable workers that generate Lines of Code each second.
- Increase worker prices after each purchase.
- Unlock later worker types through ownership requirements.
- Show a mystery preview for the next locked worker.
- Display multiplier-adjusted production per employee and per worker type.

### 4. Generic upgrade collections

- Represent active upgrades and workers with reusable model classes.
- Expose upgrades through reusable ViewModels and item templates.
- Persist purchased upgrade IDs and worker counts through collections.

### 5. Saving and offline progress

- Save and load progress as JSON in local application data.
- Save every 30 seconds, after purchases, and when the window closes.
- Calculate offline employee production with a 24-hour limit.
- Handle missing values and malformed save files without crashing.
- Preserve malformed files with a timestamped `.corrupt-*` suffix.
- Show a temporary offline-earnings message after loading.

### 6. Compact number formatting

- Format large values with suffixes such as `K`, `M`, `B`, `T`, `Qa`, and `Qi`.
- Reuse the formatter throughout the WPF interface.

### 7. Targeted and global worker-production upgrades

- Target worker-production multipliers by worker ID.
- Support multipliers that affect every worker type.
- Recalculate passive production after purchasing or loading upgrades.
- Require ownership of a targeted worker type before purchasing its upgrades.

### 8. Interface and animation polish

- Use a three-part production dashboard with a compact worker area.
- Make the entire worker card the hire action.
- Keep active upgrades separate from the worker-only scroll area.
- Show concurrent floating `+X` feedback for manual clicks.
- Pulse newly affordable active upgrades.
- Add worker-card hover, purchase, and pressed feedback.
- Animate the offline-earnings notification without shifting the dashboard.
- Store reusable Storyboards in `Styles/Animations.xaml`.

### 9. Modern dark pixel visual refresh

- Establish a deep-navy interface with restrained cyan, purple, pink, and green
  accents based on the reference material in `Design/style/`.
- Package game-ready pixel artwork for branding, Lines of Code currency, active
  upgrades, and every current worker type.
- Bundle Inter for body text and Press Start 2P for selected display headings.
- Separate shared theme resources, animations, and control styles into
  `Theme.xaml`, `Animations.xaml`, and `Controls.xaml`.
- Make purchasable active upgrades immediately recognizable with a persistent cyan
  outline and dim unavailable upgrades.
- Extract active upgrades, worker upgrades, and statistics into focused UserControls
  while retaining `MainWindow` as the application shell.
- Give the worker purchase flash its own visual layer so it remains visible while
  the worker card is hovered and another worker can still be afforded.

### 10. Expanded lifetime statistics

- Track and persist manual clicks, employees hired, and active upgrades purchased.
- Split generated Lines of Code by manual and worker sources.
- Split generated Lines of Code by online and offline production.
- Render statistic rows through a reusable `StatisticViewModel` collection.
- Add focused regression coverage for statistic accounting and persistence.

### 11. Compact worker roster and progressive reveals

- Reduce worker cards to their portrait, identity, owned count, and hire cost.
- Move per-employee and total production details into consistent tooltips.
- Use a stable three-column roster whose card spacing does not change with the
  window size or newly revealed workers.
- Keep future worker identities hidden after requirements are met and reveal them
  only when the player completes the first hire.
- Match active-upgrade tooltip styling to the worker-card tooltip presentation.

## Current priorities

### Short term

- Implement the first small set of achievements.
- Complete responsive, minimum-window-size, and high-DPI visual checks.
- Configure the supplied multi-resolution `.ico` as the executable icon.

### Longer term

- Design a Studio Level system driven by experience gain. Decide what earns
  experience, what leveling represents, and which meaningful benefits or unlocks
  studio levels provide before implementing the feature.

## Next milestones

### 1. Add the first achievements

- Begin with a small set tied to clear milestones already represented by lifetime
  statistics and progression state.
- Decide whether early achievements are recognition-only or provide modest rewards
  before adding reward logic.
- Persist earned achievement IDs and present achievements in a compact interface
  that can grow gradually.

### 2. Balance the existing gameplay

Balancing is deliberately deferred until the current systems are more complete.
Use `BALANCING_NOTES.md` as the starting point for a fresh-save playthrough and tune:

- Active-upgrade costs and multipliers.
- Worker costs, production, and cost growth.
- Worker unlock requirements.
- The transition from manual clicking to passive production.
- Progression variety beyond repeated price and value doubling.

### 3. Evolve save-data compatibility

- Add explicit save-data versioning when the first migration becomes necessary.
- Introduce migration rules once preserving development saves becomes worthwhile.
- Consider atomic or backup-based saving if the persistence system grows.

## Later possibilities

- Expand achievements beyond the initial set.
- More worker and active-upgrade types.
- Additional artwork consistent with the modern dark pixel visual direction.
- Sound effects and music controls.
- Broader studio progression building on the future Studio Level system.
- Prestige or new-game-plus after the main progression loop is established.
