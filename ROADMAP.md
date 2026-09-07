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

- Use a three-part production dashboard with a compact two-column worker area.
- Make the entire worker card the hire action.
- Keep active upgrades separate from the worker-only scroll area.
- Show concurrent floating `+X` feedback for manual clicks.
- Pulse newly affordable active upgrades.
- Add worker-card hover, purchase, and pressed feedback.
- Animate the offline-earnings notification without shifting the dashboard.
- Store reusable Storyboards in `Styles/Animations.xaml`.

## Next milestones

### 1. Adopt the Style D visual direction

Gradually reskin the existing interface using `Design/style-d/` as the visual
source of truth and the game-ready artwork in
`src/GameStudioClicker.Wpf/Assets/`. Preserve the current gameplay and layout
behavior while introducing the new look in small, reviewable slices.

#### 1.1 Establish shared theme resources

- Add `Styles/Theme.xaml` and merge it into `App.xaml` before the animation
  resources.
- Define semantic brushes for the Style D navy, mint, blue, and warm off-white
  palette rather than repeating color values throughout individual views.
- Configure crisp image rendering, layout rounding, and pixel snapping where
  appropriate.
- Confirm PNG files under the WPF `Assets` folder are packaged as resources.

#### 1.2 Reskin reusable interface components

- Apply the shared palette to the main window, panels, buttons, worker cards,
  active-upgrade buttons, tooltips, navigation, and notifications.
- Use mint for positive actions and currency, blue for technology and selection,
  and warm off-white for neutral text.
- Prefer firm edges, restrained shading, and small corner radii; avoid glow,
  gradients, glossy effects, and excessive shadows.
- Keep reusable control styles separate from animation Storyboards as the style
  collection grows.

#### 1.3 Add global branding and currency artwork

- Replace the text-only game heading with the supplied Game Studio Clicker logo.
- Add the Lines of Code icon to the primary resource display, then evaluate
  whether smaller cost displays benefit from it without becoming visually busy.
- Display pixel artwork at whole-number sizes with nearest-neighbor scaling.

#### 1.4 Add artwork to upgrade and worker cards

- Keep image selection in the WPF presentation layer rather than the Core models
  or save data.
- Map the Mechanical Keyboard upgrade ID to its supplied artwork.
- Map the Intern worker ID to its supplied portrait.
- Preserve generic and mystery fallbacks for entries that do not yet have art.
- Extend the same ID-to-asset approach as more Style D artwork is added.

#### 1.5 Refine typography, spacing, and remaining surfaces

- Add bundled, appropriately licensed fonts before relying on the pixel and
  retro typefaces shown in the reference material.
- Reserve display-style typography for headings and important values while
  keeping descriptions and tooltips readable.
- Apply the shared theme to the Statistics view and review existing animation
  colors against the new palette.
- Generate a multi-resolution Windows `.ico` from the app-icon artwork and use
  it for the executable and window.

#### 1.6 Verify the completed reskin

- Build after each visual slice and perform focused manual checks rather than
  adding unit tests for cosmetic XAML changes.
- Check the interface at 1200 x 800, the 700 x 500 minimum size, and with high-DPI
  scaling.
- Preview artwork on both approved navy backgrounds and compare the finished UI
  with the Style D reference before considering the milestone complete.

### 2. Add statistics and permanent unlock progress

Track cumulative values that are not reduced when the player spends Lines of Code.
Start with one statistic, persist it, and expose it through the ViewModel before
building a complete statistics interface.

Possible statistics:

- Lifetime Lines of Code generated.
- Lines generated manually.
- Lines generated by employees.
- Total manual clicks.
- Total employees hired.
- Total upgrades purchased.

Lifetime statistics can later support permanent content reveals, achievements, and
eventually prestige.

### 3. Balance the existing gameplay

Balancing is deliberately deferred until the current systems are more complete.
Use `BALANCING_NOTES.md` as the starting point for a fresh-save playthrough and tune:

- Active-upgrade costs and multipliers.
- Worker costs, production, and cost growth.
- Worker unlock requirements.
- The transition from manual clicking to passive production.
- Progression variety beyond repeated price and value doubling.

### 4. Evolve save-data compatibility

- Add explicit save-data versioning when the first migration is needed.
- Preserve compatibility with older saves when statistics are introduced.
- Consider atomic or backup-based saving if the persistence system grows.

## Later possibilities

- Achievements.
- More worker and active-upgrade types.
- Additional Style D artwork for active upgrades and workers.
- Sound effects and music controls.
- A fuller studio-progression theme.
- Prestige or new-game-plus after the main progression loop is established.
