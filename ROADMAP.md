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

### 12. First achievements

- Add four recognition-only achievements tied to manual clicking, hiring an
  employee, purchasing an active upgrade, and lifetime Lines of Code.
- Evaluate achievement progress from existing statistics and progression state.
- Persist earned achievement IDs and restore them safely from JSON save data.
- Present achievements as progress tiles on a dedicated page separate from
  lifetime statistics.
- Add independent navigation for workers, statistics, and achievements.
- Queue newly earned achievements and show them through an animated, non-blocking
  unlock notification.

### 13. Expanded worker roster

- Expand the worker progression to six tiers with Engineering Manager and Studio
  Director.
- Continue the prerequisite chain, production growth, and provisional cost curve.
- Add matching worker portraits and use one consistent portrait path convention.
- Fill the stable worker roster as two rows of three cards.

### 14. Audio, soundtrack, and persistent settings

- Keep soundtrack and sound-effect playback in a dedicated WPF `AudioService`.
- Play five shuffled soundtrack tracks without repeating a track until the shuffle
  is refilled, and show the current track title in the interface.
- Add sound effects for manual coding, navigation, worker and active-upgrade
  purchases, achievement unlocks, and enabled-button hover feedback.
- Reuse individual media players so rapid repetitions of the same cue restart
  instead of creating unbounded overlapping player instances.
- Provide separate music and sound-effect volume controls with mute support and
  smooth real-time soundtrack volume changes.
- Persist audio percentages through `IApplicationSettingsRepository` and
  `JsonApplicationSettingsRepository`, separate from the core game save.
- Restore settings before soundtrack playback, save changes immediately, clamp
  loaded percentages, and recover safely from missing or malformed settings files.

### 15. Shared control styling and interaction polish

- Share a rounded progress-bar template between Studio XP and achievements,
  retaining the green earned-achievement appearance.
- Match settings sliders and their hover/drag feedback to the game theme.
- Use `ClickDragSlider` to support clicking the track and immediately dragging
  without releasing the mouse button.
- Apply a shared slim vertical scrollbar style to the existing ScrollViewers.

## Current priorities

### Short term

- Implement the Studio Level and location progression plan below as the next
  feature milestone.
- Add focused regression tests for achievement unlocking, progress, and
  persistence (assistant-owned follow-up, not a prerequisite for Studio Level).
- Achievement behavior, notification order/save restoration, and expanded-worker
  progression have been manually verified by the user.
- Keep the temporary elevated click power as a development aid while separating it
  from the intended starting balance before release.
- Defer responsive, minimum-window-size, and high-DPI layout fixes until the new
  progression panels and broader interface structure settle. Current sizing and
  scaling behavior is known to need improvement.
- Configure the supplied multi-resolution `.ico` as the executable icon.
- Record the source and license for every bundled soundtrack and sound effect.
- Stress-test rapid clicking and fast button-to-button hovering, then tune cue
  volume or restart behavior only if the result sounds harsh.
- Decide whether unavailable actions need a distinct sound; disabled buttons
  currently remain silent.

### Longer term

- Expand location progression from a Bedroom/Shed through offices, corporate
  headquarters, skyscrapers, and eventually an outer-space studio.

## Studio Level and locations — active implementation plan

Each checkbox is a small task or verification checkpoint. Design choices and
recommended numbers are provisional until confirmed and playtested.

### Design direction

- [x] Agree that Studio Level represents the studio's growth and maturity, with
  location upgrades as major milestone rewards.
- [x] Award XP across active gameplay, but not directly from passive worker
  production. Offline production likewise awards no direct XP.
- [x] Use locations to gate worker tiers: Bedroom/Shed allows Interns; Small
  Office allows Junior and Senior Developers.
- [ ] Confirm the proposed relocation rule: meeting the level requirement makes
  relocation available; the player then pays Lines of Code to move.
- [x] Confirm provisional thresholds, action rewards, and relocation cost below
  (agreed 2026-09-16; balance after further implementation).
- [ ] Decide whether intermediate levels need rewards beyond progress toward the
  next location. Do not assume a global production bonus for the first version.

### Phase 1 — provisional first-version values

Recommended cumulative thresholds (total XP, not XP required separately per level):

| Studio Level | Total XP required |
|---|---:|
| 1 | 0 |
| 2 | 100 |
| 3 | 250 |
| 4 | 500 |
| 5 | 900 |

Recommended XP rewards:

| Action | XP |
|---|---:|
| Successful manual coding click | 1, regardless of Lines per Click |
| Intern / Junior / Senior hire | 10 / 25 / 50 |
| Lead / Engineering Manager / Studio Director hire | 100 / 200 / 400 |
| Active upgrade in content Era 1 / 2 / 3 / 4 | 25 / 50 / 100 / 150 |
| First-time achievement unlock | 25 |
| Passive production, offline production, failed purchase, UI navigation | 0 |

- [ ] Start in `bedroom` at Level 1, with Interns available.
- [ ] Make `small_office` available at Level 5 for 5,000 Lines of Code; unlock
  Junior and Senior Developer tiers after relocation (their hire costs still apply).
- [ ] Give relocation no XP initially, avoiding an automatic relocation/XP chain.
- [ ] Keep Levels 1–5 and these two locations as the first playable slice.
- [ ] Evaluate pace with normal starting click power, not the development override.
  Aim for the first relocation to feel like an early milestone rather than a long
  click grind; tune after a fresh-save playthrough.

### Phase 2 — core leveling model

- [x] Create `Models/StudioProgression.cs` in Core, responsible only for XP and
  level calculations.
- [x] Add `TotalExperience`, derived `Level`, `ExperienceIntoCurrentLevel`, and
  `ExperienceNeededForNextLevel`, using explicit cumulative thresholds.
- [x] Add `AddExperience(long amount)` and `RestoreExperience(long amount)`.
- [x] Handle negative values, multiple-level gains, and the prototype maximum
  level. Retain total XP beyond Level 5 so later threshold additions can use it.
- [ ] Assistant adds a few focused leveling-rule regression tests after the model
  is implemented; the user is not assigned test-writing.

### Phase 3 — connect one XP source

- [x] Let `GameState` own a `StudioProgression` instance.
- [x] Award one XP per successful `WriteCode()` action, independent of click power.
- [x] Expose level and XP progress through `MainViewModel`, refreshing them after
  relevant actions.
- [x] Verify manual-click XP through the in-game level/XP display (user-reported).
  A debugger is optional; boundary checks remain listed in Phase 5.

### Phase 4 — persist XP

- [x] Add `StudioExperience` to `GameSaveData`.
- [x] Update `CreateSaveData()` and `RestoreFromSaveData()`; save XP, not a separate
  level value.
- [x] Default missing XP in older saves to zero and clamp invalid negative XP.
- [x] Verify XP and derived level survive closing and restarting the game
  (user-reported).

### Phase 5 — compact level display

- [x] Review `Design/style/` and WPF `Assets/` before changing the visual design.
- [x] Add a compact dashboard level label, XP text, and progress bar; defer a full
  location screen.
- [x] Implement maximum-level text and a full progress bar, using style triggers.
- [ ] Finish explicit boundary checks for the initial level, partial progress, level transition, and maximum
  level presentation.

### Control styling follow-up

- [x] Move rounded XP and achievement progress-bar styles into `Styles/Controls.xaml`.
- [x] Style settings sliders and verify click-and-drag interaction (user-reported).
- [x] Style vertical scrollbars through the shared control resources.
- [x] Confirm mouse-wheel scrolling, thumb dragging, and track/page clicks in each
  scrollable view.

### Phase 6 — remaining XP sources

- [ ] Add explicit `ExperienceReward` values to worker content and award hire XP
  only after a successful purchase.
- [ ] Add explicit active-upgrade rewards and award XP only on successful purchase.
- [ ] Add first-time achievement rewards without awarding XP again on restoration.
- [ ] Verify each source separately, including failed purchases and simultaneous
  achievement/other rewards. Passive and offline code generation award no direct
  XP; a newly earned achievement may still grant its one-time bonus.
- [ ] Assistant adds focused action-reward and duplicate-award regression tests.

### Phase 7 — location model and rules

- [ ] Create Core `Models/StudioLocation.cs` to describe a location's `Id`,
  `DisplayName`, required level, relocation cost, and available worker IDs.
- [ ] Define Bedroom/Shed and Small Office through `GameContentFactory`.
- [ ] Add current location and `CanRelocate` / `TryRelocate` rules to `GameState`.
- [ ] Require sufficient level and currency, reject the current/earlier location,
  and deduct the cost only on success. Moving forward retains earlier worker tiers.
- [ ] Keep existing worker gates unchanged until location persistence and migration
  are ready.

### Phase 8 — location persistence and existing saves

- [ ] Add `CurrentLocationId` to `GameSaveData` and restore valid IDs safely.
- [ ] Define missing/unknown-ID fallback and migration for saves already owning
  higher-tier workers, so existing employees do not become invalid or disappear.
- [ ] Decide whether this migration warrants the first explicit save-data version.
- [ ] Assistant adds focused relocation and save-compatibility tests.

### Phase 9 — relocation interface

- [ ] Expose current/next location, requirements, cost, and relocation command from
  the ViewModel.
- [ ] Add a focused location panel or `StudioView`, beginning with text and existing
  resources rather than new artwork.
- [ ] Show unmet requirements and which worker tiers relocation makes available.
- [ ] Verify unavailable, affordable, successful, and already-completed relocation.

### Phase 10 — location-based worker gates

- [ ] Make current location the primary worker-tier gate in Core rules and UI.
- [ ] Review existing worker-count prerequisites and remove redundant gates by
  deliberate design choice, retaining hire costs and useful mystery/reveal behavior.
- [ ] Review active-upgrade availability so later-era purchases do not undermine
  location progression or create an XP deadlock before relocation.
- [ ] Verify Intern-only starting progression, Small Office hires, continued access
  to earlier workers, and migration behavior on an existing save.

### Phase 11 — polish, expand, and balance

- [ ] Add level-up/relocation feedback and optional audio without replaying loaded
  milestones.
- [ ] Add consistent location artwork/environment changes after the rules work.
- [ ] Extend levels, locations, and future milestone rewards incrementally.
- [ ] Playtest and tune XP rewards, thresholds, and relocation costs together.
- [ ] Update README and completed roadmap milestones as each slice is delivered.
- [ ] Revisit responsive layout and Windows display scaling once the UI settles.

## Next milestones

### 1. Studio Level and location progression

- Core leveling, manual-click XP, XP persistence, and the dashboard display are
  implemented. Continue with Phase 6, starting with successful worker-hire XP.
- Keep focused regression coverage and remaining manual boundary checks visible
  as follow-ups; locations and relocation are not implemented yet.

### 2. Remaining achievement and interface follow-ups

- Manual achievement/roster behavior verification is complete, as reported by the
  user. Focused automated achievement/save-compatibility coverage remains pending.
- Minimum-size and display-scaling fixes are explicitly deferred until the new
  progression UI and broader interface structure settle.

### 3. Balance the existing gameplay

Balancing is deliberately deferred until the current systems are more complete.
Use `BALANCING_NOTES.md` as the starting point for a fresh-save playthrough and tune:

- Active-upgrade costs and multipliers.
- Worker costs, production, and cost growth.
- Worker unlock requirements.
- The transition from manual clicking to passive production.
- Progression variety beyond repeated price and value doubling.

### 4. Evolve save-data compatibility

- Add explicit save-data versioning when the first migration becomes necessary.
- Introduce migration rules once preserving development saves becomes worthwhile.
- Consider atomic or backup-based saving if the persistence system grows.
- Defer numeric overflow handling to a coordinated cleanup across currency,
  lifetime statistics, production multipliers, worker costs/counts, and Studio XP.
  Choose one consistent policy rather than adding an XP-only guard now.

## Later possibilities

- Expand achievements beyond the initial recognition-only set and decide whether
  later achievements should grant rewards.
- More worker and active-upgrade types.
- Additional artwork consistent with the modern dark pixel visual direction.
- Broader studio progression building on the future Studio Level system.
- Prestige or new-game-plus after the main progression loop is established.
