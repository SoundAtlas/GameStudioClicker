GAME STUDIO CLICKER - NEW MOOD BOARD ASSET PACK (V3)
=====================================================

VISUAL DIRECTION
----------------
This asset set is based on the new "Overall style inspiration" mood board.

- Clean, readable pixel art with modern-dark game UI presentation
- Cozy game-development personality and expressive, useful silhouettes
- Deep navy surfaces with controlled cyan, purple, pink, and green accents
- Crisp square pixel clusters with selective highlights and restrained glow
- A balance between nostalgic pixel art and polished contemporary UI
- Transparent PNG backgrounds for flexible placement in the WPF interface

REFERENCE PALETTE
-----------------
Background:      #0B1220
Surface:         #121A2B
Primary cyan:    #00E5FF
Secondary purple:#7C3AED
Accent pink:     #FF6EC7
Success green:   #22C55E
Warning amber:   #F59E0B
Error red:       #EF4444
Text primary:    #F8FAFC
Text secondary:  #94A3B8
Border:          #1E293B
Glow blue:       #36BDF8

TYPOGRAPHY DIRECTION
--------------------
Use Press Start 2P, or a similarly readable block-pixel display face, for
headings and logo-supporting labels. Use Inter for body text and dense UI.
Keep letter spacing comfortable and avoid long passages in the pixel font.

ASSETS AND INTENDED USAGE
-------------------------

1. Assets/Branding/Logo/logo_game_studio_clicker.png
   Main logo based on the mood board's recommended Variant 1 direction.
   Intended for title screens, splash screens, menus, and wide headers.
   Supplied master canvas: 1024 x 512 px.
   Recommended display width: 420-760 px, preserving aspect ratio.

2. Assets/Branding/AppIcon/app_icon_game_studio_clicker.png
   Compact CRT-and-code emblem based on the mood board's icon variation.
   Intended for the app icon, launcher, taskbar, and small brand badges.
   Supplied master canvas: 512 x 512 px.
   Recommended exports: 256, 128, 64, 48, 32, and 16 px square.

3. Assets/Icons/Currency/icon_lines_of_code.png
   Success-green code glyph for Lines of Code balances, gains, costs,
   tooltips, and floating rewards.
   Supplied master canvas: 128 x 128 px.
   Recommended in-game display size: 16-40 px square.

4. Assets/Upgrades/Hardware/upgrade_mechanical_keyboard.png
   Mechanical Keyboard artwork for upgrade cards, shop rows, unlock messages,
   the Latest Upgrades panel, and detail views.
   Supplied master canvas: 512 x 384 px.
   Recommended in-game display size: 64 x 48 to 128 x 96 px.

5. Assets/Workers/Intern/worker_intern.png
   Intern portrait with laptop and coffee for worker cards, roster entries,
   unlock notifications, and worker detail views.
   Supplied master canvas: 512 x 512 px.
   Recommended in-game display size: 72-128 px square.

CONSISTENCY RULES FOR FUTURE ASSETS
-----------------------------------

1. Treat the new mood board as the source of truth for palette, sprite density,
   outline weight, lighting, and personality.

2. Build forms from deliberate square pixel clusters. Keep diagonals stepped
   and silhouettes readable; do not apply a generic pixelation filter to
   smooth artwork.

3. Use #0B1220 and #121A2B for structural darks. Use #00E5FF as the dominant
   interactive/technology accent, with #36BDF8 for limited edge light.

4. Purple and pink are secondary accent colors. Use them sparingly on premium
   hardware, special states, or small points of visual interest.

5. Use #22C55E consistently for Lines of Code, positive resource changes,
   success states, and progression confirmation.

6. Amber and red communicate warning and error states. They may also appear in
   cozy physical props such as coffee mugs or warm room lighting, but should
   not compete with cyan as the main interface color.

7. Keep reusable assets on genuine transparent backgrounds. Do not bake in
   card frames, labels, prices, rarity borders, or full UI panels.

8. A subtle object-local glow is acceptable where the mood board uses neon
   light, but avoid large blurry halos that reduce readability on navy panels.

9. Keep worker portraits expressive and role-specific. Preserve consistent
   proportions, outline weight, and face scale across worker tiers.

10. Scale with nearest-neighbor interpolation. In WPF, use whole-number layout
    sizes where practical and enable nearest-neighbor bitmap scaling.

11. Name files in lowercase snake_case by category and subject, for example
    upgrade_gaming_mouse.png, worker_junior_developer.png, or icon_save.png.

12. Before shipping, preview each asset on both #0B1220 and #121A2B and at its
    smallest intended display size. Check silhouette, contrast, and pixel-edge
    clarity rather than judging only the large master image.

