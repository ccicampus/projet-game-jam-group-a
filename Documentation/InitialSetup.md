# Initial Project Setup Guide

Complete setup instructions for configuring your Unity 2D game project. Follow these steps after creating the folder structure.

**Time to Complete:** 15-20 minutes
**Required Unity Version:** 6.2 (6000.3.2f1)

---

## 1. Layers Setup

Layers organize objects for rendering order, physics interactions, and culling.

### Create Layers

Go to **Edit → Project Settings → Tags and Layers** and create these layers in order:

| Layer # | Name | Purpose |
|---------|------|---------|
| 0 | Default | Default layer for unassigned objects |
| 1-5 | (leave empty) | Reserved for future use |
| 6 | Ground | Platforms, walls, solid surfaces |
| 7 | Player | Player character |
| 8 | Enemy | Enemy characters |
| 9 | Projectile | Bullets, missiles, thrown objects |
| 10 | Items | Collectibles, power-ups, loot |
| 11 | UI | UI elements (usually on separate Canvas) |
| 12 | Background | Background elements (parallax layers) |
| 13 | Foreground | Foreground elements (in front of player) |

### Update Constants.cs

Update `Assets/_Project/Scripts/Utilities/Constants.cs` with your layer setup:

```csharp
// Layers
public const int LAYER_GROUND = 6;
public const int LAYER_PLAYER = 7;
public const int LAYER_ENEMY = 8;
public const int LAYER_PROJECTILE = 9;
public const int LAYER_ITEMS = 10;
public const int LAYER_UI = 11;
public const int LAYER_BACKGROUND = 12;
public const int LAYER_FOREGROUND = 13;
```

---

## 2. Tags Setup

Tags are labels for identifying objects. Create these tags in **Edit → Project Settings → Tags and Layers**:

| Tag | Usage |
|-----|-------|
| Player | Main player character |
| Enemy | Enemy characters |
| Ground | Platforms/walkable surfaces |
| Collectible | Pickup items |
| Projectile | Bullets, thrown objects |
| Hazard | Spikes, lava, etc. |
| Door | Level transition doors |
| Checkpoint | Save point |
| Respawn | Respawn point |
| Trigger | Generic trigger zones |

### Update Constants.cs

```csharp
// Tags
public const string TAG_PLAYER = "Player";
public const string TAG_ENEMY = "Enemy";
public const string TAG_GROUND = "Ground";
public const string TAG_COLLECTIBLE = "Collectible";
public const string TAG_PROJECTILE = "Projectile";
public const string TAG_HAZARD = "Hazard";
public const string TAG_DOOR = "Door";
```

---

## 3. Sorting Layers Setup

Sorting layers control the z-order of 2D sprites.

Go to **Edit → Project Settings → Tags and Layers** → **Sorting Layers** tab:

Create these sorting layers in order (top = rendered first):

| Order | Layer Name | Purpose |
|-------|-----------|---------|
| 0 | Background | Parallax backgrounds, far scenery |
| 1 | Midground | Mid-distance scenery |
| 2 | Ground | Ground, platforms |
| 3 | Player | Player character |
| 4 | Enemy | Enemy characters |
| 5 | Items | Collectibles, pickups |
| 6 | Projectiles | Bullets, effects |
| 7 | Foreground | Foreground elements (in front) |
| 8 | UI | UI overlays (if not using Canvas) |

### Why This Order?
- **Background first** → rendered behind everything
- **Player/Enemy middle** → main gameplay objects
- **Foreground last** → drawn on top
- **UI topmost** → always visible

---

## 4. Physics2D Settings

Configure 2D physics for consistent behavior.

Go to **Edit → Project Settings → Physics2D**:

### Gravity Settings
- **Gravity:** `(0, -9.81)` - Standard gravity pointing down
- **Default Material:** Create a new Physics2D Material for default friction/bounce

### Collision Settings
- **Max Linear Correction:** `0.2`
- **Default Solver Iterations:** `8` (higher = more stable, more expensive)
- **Default Time Step:** `0.016667` (60 FPS)

### Layer Collision Matrix
Set up which layers collide with each other:

```
Ground:      ✓ Ground, ✓ Player, ✓ Enemy, ✓ Projectile
Player:      ✓ Ground, ✗ Player, ✓ Enemy, ✓ Projectile, ✓ Items
Enemy:       ✓ Ground, ✓ Player, ✗ Enemy, ✓ Projectile
Projectile:  ✓ Ground, ✓ Enemy, ✗ Projectile
Items:       ✗ (use Trigger colliders - no physics)
Background:  ✗ (no colliders)
```

---

## 5. Scene Structure

### _Main Scene (Persistent Manager Scene)

Create `Assets/_Project/Scenes/_Main.unity` with this structure:

```
_Main
├── Managers (Empty GameObject)
│   ├── GameManager (Script)
│   ├── InputManager (Script)
│   ├── AudioManager (Script)
│   ├── UIManager (Script)
│   ├── PoolingManager (Script)
│   └── SceneTransitionManager (Script)
│
├── Canvas (UI Root)
│   ├── MainMenuPanel
│   ├── GameplayHUD
│   ├── PauseMenuPanel
│   └── GameOverPanel
│
└── PersistentAudio
    └── AudioListener
```

### Build Settings
1. Open **File → Build Settings**
2. Drag `Assets/_Project/Scenes/_Main.unity` to the top as **Scene 0**
3. Add other level scenes below (e.g., Scene 1, Scene 2, etc.)
4. Platform: **PC, Mac & Linux Standalone**
5. Resolution: **1920×1080** (16:9)

---

## 6. Canvas & UI Setup

### Canvas Configuration

For your main UI Canvas:

**Canvas Properties:**
- **Render Mode:** Screen Space - Overlay
- **Canvas Scaler:**
  - UI Scale Mode: Scale with Screen Size
  - Reference Resolution: 1920 × 1080
  - Match: Width or Height (depends on design)

**Canvas Rect Transform:**
- Scale: (1, 1, 1)
- Anchors: Full screen (min: 0,0 / max: 1,1)

### UI Panels Structure

Each panel should be a child of Canvas:

```
Canvas
├── MainMenuPanel
│   ├── Title (Text)
│   ├── PlayButton (Button)
│   └── QuitButton (Button)
│
├── GameplayHUD (Always visible)
│   ├── ScoreText
│   ├── HealthBar
│   └── TimerText
│
├── PauseMenuPanel (Disabled by default)
│   ├── PauseTitle
│   ├── ResumeButton
│   └── QuitButton
│
└── GameOverPanel (Disabled by default)
    ├── GameOverTitle
    ├── FinalScoreText
    └── RestartButton
```

---

## 7. Audio Setup

### AudioMixer Configuration

1. Create: **Assets/_Project/Audio/AudioMixer.mixer**
2. Structure:
```
Master (0 dB)
├── Music (-20 dB default)
├── SFX (-15 dB default)
└── Ambient (-25 dB default)
```

3. **Edit → Project Settings → Audio**:
   - Default Output: (None - will be set in game)
   - Spatial Audio: OFF (unless 3D audio needed)

### AudioManager Setup

In `_Main` scene, select the `AudioManager` GameObject:
- Drag audio clips to exposed fields in inspector
- Set mixer reference
- Test volume levels

---

## 8. Input System Setup

### Modern Input System (Already Configured)

The project uses Modern Input System (NOT legacy InputManager).

**Verify in Edit → Project Settings → Input System Package:**
- UI/Input Module: Use Modern Input System
- NOT the old InputManager

### Default Input Actions

Your InputManager script should handle:
- **Movement:** WASD / Arrow Keys / Gamepad D-Pad
- **Jump:** Space / Gamepad A Button
- **Interact:** E / Gamepad X Button
- **Pause:** Esc / Gamepad Menu Button

---

## 9. Game Settings (ProjectSettings)

### Quality Settings
1. **Edit → Project Settings → Quality**
   - Target Frame Rate: **60**
   - V-Sync Count: **Off**
   - Enable GPU Instancing: **ON**

### Time Settings
1. **Edit → Project Settings → Time**
   - Fixed Timestep: **0.016667** (60 FPS)
   - Time Scale: **1.0**

### Player Settings
1. **Edit → Project Settings → Player**
   - Company Name: Your team name
   - Product Name: "The Masked Visitor"
   - Resolution: **1920×1080**
   - Full-screen Mode: Windowed or Fullscreen (your choice)
   - Default Cursor: (your cursor sprite)

---

## 10. First Scene Setup Checklist

Before you start developing, verify your `_Main` scene:

### GameObject Setup
- [ ] `Managers` GameObject created
- [ ] All manager scripts added to `Managers` object
- [ ] Canvas created with UI panels
- [ ] AudioListener present in scene
- [ ] Scene marked as Scene 0 in Build Settings

### Manager Configuration
- [ ] GameManager: Configured, debug mode tested
- [ ] InputManager: Input actions assigned
- [ ] AudioManager: Mixer and clips assigned
- [ ] UIManager: UI panels assigned
- [ ] PoolingManager: Pool prefabs registered
- [ ] SceneTransitionManager: Scene names in Constants.cs

### Physics Setup
- [ ] Gravity set to (0, -9.81)
- [ ] Layer collision matrix configured
- [ ] Default 2D physics material created

### Project Settings
- [ ] Layers created (6-13)
- [ ] Tags created
- [ ] Sorting layers created in order
- [ ] Build Settings configured (Scene 0 = _Main)
- [ ] Resolution set to 1920×1080
- [ ] Target frame rate: 60 FPS

---

## 11. Test Your Setup

### Quick Test
1. **Open _Main scene**
2. **Press Play**
3. Check Console for errors:
   - ✅ No "Missing Manager" errors
   - ✅ No script warnings
   - ✅ Input system responds
   - ✅ Audio plays without errors

### Input Test
```csharp
// Add this to a test script
void Update()
{
    if (InputManager.Instance.MoveInput.x != 0)
        Debug.Log("Input detected: " + InputManager.Instance.MoveInput);
}
```

### Audio Test
```csharp
// Test audio in AudioManager
AudioManager.Instance.PlayMusic(testAudioClip);
AudioManager.Instance.PlaySFX(testSFXClip);
```

---

## 12. Game-Specific Setup for "The Masked Visitor"

For this specific game, additionally configure:

### Scene Names (Update Constants.cs)
```csharp
public const string SCENE_MAIN = "_Main";
public const string SCENE_HALLWAY = "Hallway";
public const string SCENE_MIRROR = "Mirror";
public const string SCENE_GAME_OVER = "GameOver";
```

### Layers for 2D Parallax
- Layer 12 (Background): Far parallax layers
- Layer 2 (Midground): Mid parallax layers
- Layer 0 (Foreground): Near parallax layers
- Layer 7 (Player): Character at center depth

### Animation Parameters
Add to Constants.cs:
```csharp
public const string ANIM_WALK = "Walk";
public const string ANIM_IDLE = "Idle";
public const string ANIM_JUDGE = "Judge";
public const string ANIM_REVEAL_MASK = "RevealMask";
```

### Audio Mixer Groups
Create these in your AudioMixer:
- Master
  - Music (for hallway/ambience)
  - SFX (for mini-game sounds)
  - UI (for button clicks, timers)

---

## Troubleshooting

### "Manager not found" error
- **Fix:** Ensure `_Main` scene is at Build Settings Scene 0
- Make sure all manager scripts are on the `Managers` GameObject

### Input not responding
- **Fix:** Check Edit → Project Settings → Input System Package is using Modern Input System
- Verify InputManager script has input actions assigned

### Physics not working
- **Fix:** Check gravity is set to (0, -9.81) in Physics2D settings
- Verify colliders are on correct layers and layer collision matrix is configured

### Audio not playing
- **Fix:** Check AudioManager is in `_Main` scene
- Verify audio clips are assigned to the manager
- Check volume levels in AudioMixer

### Sprites not visible
- **Fix:** Check sorting layer is correct (not Background if you want foreground)
- Verify sprite is assigned to correct layer (6-13, not Default 0)
- Check canvas/UI isn't blocking with transparent overlay

---

## Next Steps

1. ✅ Complete this Initial Setup
2. ✅ Verify all layers, tags, and scenes
3. ✅ Test managers in _Main scene
4. ⬜ Open first gameplay scene and add a player
5. ⬜ Implement mini-game framework
6. ⬜ Start game jam development!

---

**Documentation Location:**
- See `README.md` for project structure
- See `StyleGuide.md` for coding standards
- See `C_SHARP_USAGE_GUIDE.md` for API reference
- See `GDD.md` for game design details
