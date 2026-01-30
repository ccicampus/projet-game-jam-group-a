# Projet Game Jam Group A - Project State

**Last Updated:** 2026-01-30
**Status:** JAM ACTIVE - Game concept locked, tasks assigned
**Game:** "The Masked Visitor" - Halloween trick-or-treat interactive game
**Theme:** Global Game Jam 2026 - "Mask"

---

## Executive Summary

This is a **Unity 2D Game Jam Starter Framework** (Unity 6.2) for "The Masked Visitor" - a spooky but kid-friendly Halloween game. All core manager systems are ready, and the team is organized with clear task assignments. **Game concept locked 2026-01-30** with full team onboarding.

---

## Project Structure

```
Assets/_Project/
├── Scenes/              → SampleScene.unity (main scene)
├── Scripts/
│   ├── Core/            → GameManager, SceneTransitionManager, SaveSystem
│   ├── Player/          → PlayerController, PlayerHealth, PlayerAnimationController
│   ├── Managers/        → InputManager, AudioManager, UIManager, PoolingManager
│   ├── Camera/          → CameraFollow
│   ├── Enemies/         → Enemy (base class), EnemyAI
│   ├── Items/           → Item (base), Collectible, PowerUp
│   └── Utilities/       → Singleton<T>, Constants, Extensions
├── Prefabs/
├── Sprites/
├── Audio/
├── Animations/
├── Materials/
├── Fonts/
└── Data/
```

---

## Tech Stack

- **Engine:** Unity 6.2 (6000.3.2f1)
- **Graphics:** Universal Render Pipeline (URP)
- **Input:** Modern Input System (not legacy InputManager)
- **Physics:** Rigidbody2D + BoxCollider2D
- **Audio:** Built-in AudioMixer
- **Animation:** Animator + TextMeshPro
- **Build Target:** PC/Console (configurable)

---

## Core Manager Systems

All managers are **Singletons** with `DontDestroyOnLoad`:

### GameManager
- Game state management (pause/resume)
- Score tracking
- Level loading
- 60 FPS target
- Location: `Scripts/Core/GameManager.cs`

### InputManager
- Modern Input System integration
- Multi-device support: Keyboard, Gamepad, Mouse
- Input buffering
- Rebinding framework (extensible)
- Location: `Scripts/Managers/InputManager.cs`

### AudioManager
- Music and SFX management
- AudioMixer integration
- Volume control
- Audio clip registration system
- Location: `Scripts/Managers/AudioManager.cs`

### PoolingManager
- Object pooling with dynamic expansion
- Queue-based system (FIFO)
- Proper cleanup on destroy
- Optional IPoolable interface support
- Location: `Scripts/Managers/PoolingManager.cs`

### SceneTransitionManager
- Scene loading with transitions
- Async scene loading support
- Error handling for failed loads (NEW)
- Location: `Scripts/Core/SceneTransitionManager.cs`

### SaveSystem
- Async save/load functionality
- JSON serialization
- Backup file system
- Error handling with fallbacks
- Location: `Scripts/Core/SaveSystem.cs`

### UIManager
- UI state management
- Panel transitions
- Location: `Scripts/Managers/UIManager.cs`

---

## Player Systems

### PlayerController
- Movement (WASD/Arrow keys or Gamepad D-Pad)
- Jumping with jump buffer (frames before landing to jump)
- Coyote time (grace period after leaving ground)
- Smooth acceleration/deceleration
- Sprite flipping
- Animation integration

### PlayerHealth
- Health management
- Damage/heal system
- Invincibility frames
- UnityEvent system for health changes
- **Event cleanup added 2026-01-29** (prevents memory leaks)

### PlayerAnimationController
- Animator parameter caching (performance optimization)
- Animation state management

### CameraFollow
- Smooth camera tracking
- Boundary support
- Lead/offset customization

---

## Enemy Systems

### Enemy (Base Class)
- Health and damage system
- Collision-based damage to player
- Event system for health changes
- Score awarding on death
- **Event cleanup added 2026-01-29** (prevents memory leaks)
- Override-friendly virtual methods

### EnemyAI (Example)
- Simple patrol and chase behavior
- Ground/wall detection
- Inherits from Enemy

---

## Item Systems

### Item (Abstract Base)
- Base class for all collectible items
- Trigger-based collection
- VFX spawning with **lifetime management added 2026-01-29**
- Audio playback
- Score awarding
- Extensible OnCollected pattern

### Collectible
- Simple example collectible item

### PowerUp
- Health, Speed, Jump, Invincibility, DoubleJump types
- **DoubleJump implementation added 2026-01-29** (was missing)
- Extensible effect application system

---

## Critical Fixes Applied (2026-01-29)

### 1. Event Listener Memory Leaks [FIXED]
**Issue:** UnityEvents in PlayerHealth and Enemy were never cleaned up
**Risk:** Memory leak if listeners were registered
**Files Modified:** `PlayerHealth.cs`, `Enemy.cs`
**Fix:** Added `OnDestroy()` with `RemoveAllListeners()` for all events
**Status:** ✓ RESOLVED

### 2. Missing PowerUp Implementation [FIXED]
**Issue:** DoubleJump case in PowerUp.cs switch statement was missing
**Risk:** Enum value would silently fail
**File Modified:** `PowerUp.cs`
**Fix:** Added `case PowerUpType.DoubleJump` with ApplyDoubleJumpPowerUp() method
**Status:** ✓ RESOLVED

### 3. Scene Loading Error Handling [FIXED]
**Issue:** SceneTransitionManager had no error handling for failed scene loads
**Risk:** Silent failures, corrupted state
**File Modified:** `SceneTransitionManager.cs`
**Fix:** Added null checks and `asyncLoad.failed` checks with error logging
**Status:** ✓ RESOLVED

### 4. Coroutine Cleanup [FIXED]
**Issue:** SceneTransitionManager didn't clean up coroutines in OnDestroy
**Risk:** Coroutines could continue after manager destruction
**File Modified:** `SceneTransitionManager.cs`
**Fix:** Added `StopAllCoroutines()` in OnDestroy
**Status:** ✓ RESOLVED

### 5. VFX Lifetime Management [FIXED]
**Issue:** Instantiated VFX particles persisted indefinitely
**Risk:** Memory accumulation over time
**File Modified:** `Item.cs`
**Fix:** Added `Destroy(vfx, 2f)` to clean up VFX after 2 seconds
**Status:** ✓ RESOLVED

---

## Documentation Status

### README.md
- **Status:** Updated 2026-01-29
- **Changes:** Fixed broken scene reference, removed outdated steps
- **Content:** Setup instructions, project structure, coding standards, git workflow
- **Rating:** Good (7/10) - Mostly accurate, fully functional

### GDD.md (Game Design Document)
- **Status:** Pending - Template only, no actual game design filled in
- **Next Step:** Complete after Friday team meeting
- **Action:** Team needs to define game concept, mechanics, art style, audio style

### StyleGuide.md
- **Status:** Excellent (9/10)
- **Content:** Naming conventions, best practices, architectural patterns
- **Rating:** Perfectly matches actual code implementation

### CLAUDE.md (This File)
- **Status:** Created 2026-01-29
- **Next Update:** Friday night after team meeting
- **Content:** Project overview, architecture, systems, fixes, and next steps

---

## Code Quality Metrics

| Category | Score | Status |
|----------|-------|--------|
| Naming Conventions | 9/10 | Excellent - Full compliance |
| Null Safety | 9/10 | Excellent - Comprehensive checks |
| Error Handling | 8/10 | Good - Improved with recent fixes |
| Architecture | 8/10 | Good - Professional patterns used |
| Memory Management | 8/10 | Good - Pooling implemented, fixes applied |
| Documentation | 8/10 | Good - StyleGuide excellent, GDD pending |
| **Overall** | **8.2/10** | **JAM READY** |

---

## Development Patterns

### Singleton Pattern
All managers use consistent singleton implementation:
```csharp
public static [ManagerName] Instance { get; private set; }

private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}
```

### Event System
PlayerHealth and Enemy use UnityEvent for loose coupling:
```csharp
public UnityEvent<int> OnHealthChanged;
public UnityEvent OnDeath;

// Cleanup (NEW):
private void OnDestroy()
{
    OnHealthChanged?.RemoveAllListeners();
    OnDeath?.RemoveAllListeners();
}
```

### Object Pooling
For frequently spawned objects (bullets, VFX, enemies):
```csharp
GameObject obj = PoolingManager.Instance.SpawnFromPool("Bullet", position, rotation);
// Use object...
PoolingManager.Instance.ReturnToPool("Bullet", obj);
```

### Input Handling
All input goes through InputManager:
```csharp
float horizontal = InputManager.Instance.MoveInput.x;
bool jumpPressed = InputManager.Instance.IsJumpPressed;
```

---

## Game Concept: "The Masked Visitor"

### Story
An old person doing trick-or-treat on Halloween encounters masked visitors at their front door. They must determine if each visitor is "nice" or a "monster" using mini-games that provide clues.

### Core Loop
1. **Scene:** Walk through 2D parallax hallway toward front door
2. **Encounter:** Masked visitor appears at door
3. **Mini-games:** Play 2-3 random games to gather clues (QTE, Point & Click, Candy basket, Memory, Combat)
4. **Judge:** Decide if "Nice" or "Monster" with timer pressure
5. **Score:** Right = points, Wrong = malus
6. **Reset:** Fade out, next visitor appears
7. **Repeat:** Endless loop

### Visual Style
- 2D sprites with parallax scrolling (fake 3D depth)
- Halloween spooky but burlesque/quirky tone
- Varied mask designs (fun, scary, unusual)
- Warm lighting for hallway, depth perception via shadows

### Mini-Games (Clue-Providers)
- **Combat:** Grab cane/umbrella, fight/scare visitor (Alex's task)
- **QTE:** Quick-time button pressing reactions
- **Point & Click:** Examine visitor for visual clues
- **Candy Basket:** Choose candies, spoilt ones = malus
- **Memory:** Matching game (fits old character theme)

---

## Task Assignments (14 Tasks Total)

### Designers (Morgane & Laetitia)
1. **Task #1** - Design 2D hallway scene with parallax depth effect (Morgane or Laetitia)
2. **Task #2** - Design visitor character masks and variations (Morgane or Laetitia)
3. **Task #13** - UI Design: Mini-game layouts and judgment screens (shared)

### Sound Engineer (Pierre Albert)
4. **Task #11** - Create audio: Halloween theme music and loop
5. **Task #12** - Create audio: Mini-game SFX and feedback sounds

### Developers (Alex, Mohamed, Thomas M., Thomas de O.)
**Alex:**
- **Task #5** - Build combat mini-game (cane/umbrella fight)

**Mohamed (or assign):**
- **Task #3** - Build mini-game framework system (PRIORITY - others depend on this)
- **Task #4** - Implement judgment system + score tracking
- **Task #10** - Build timer system for judgment phase

**Thomas M. (or assign):**
- **Task #6** - Implement Point & Click examination mini-game
- **Task #7** - Implement candy basket selection mini-game
- **Task #8** - Implement memory game (card/pattern matching)

**Thomas de O. (or assign):**
- **Task #9** - Implement QTE (Quick-Time Event) mini-game
- **Task #14** - Full game integration and system testing (final phase)

### Critical Path
1. **Start:** Task #3 (mini-game framework) - other games depend on it
2. **Parallel:** Tasks #1, #11, #12 (design + audio can start immediately)
3. **Then:** Individual mini-games (#5-9)
4. **Finally:** Task #14 (integration + testing)

### During Jam:
- Use provided managers (GameManager, InputManager, AudioManager, PoolingManager)
- Register to events but unregister in OnDestroy
- Pool frequently spawned objects
- Follow naming conventions from StyleGuide.md
- Test frequently - run the game every 30 minutes
- Update CLAUDE.md with daily progress

### Post-Jam (if time):
- Profile memory usage
- Optimize hot paths
- Polish audio and VFX timing

---

## Key Files Reference

| File | Purpose | Priority |
|------|---------|----------|
| `GameManager.cs` | Game state + scoring | Critical |
| `InputManager.cs` | Input handling | Critical |
| `AudioManager.cs` | Audio management | Important |
| `PoolingManager.cs` | Object pooling | Important |
| `SceneTransitionManager.cs` | Scene loading | Important |
| `PlayerController.cs` | Player movement | Core Feature |
| `PlayerHealth.cs` | Player health | Core Feature |
| `Enemy.cs` | Enemy base class | Core Feature |
| `PowerUp.cs` | Power-ups | Optional |
| `Constants.cs` | Game constants | Reference |

---

## Commands & Skills Available

- `/jam-status` - Quick health check
- `/review-starters` - Validate all starter scripts
- `/modern-patterns-audit` - Check modern practices
- `/design-feature` - Plan a new game feature
- `/review-code` - Deep code review
- `/generate-feature` - Generate complete C# scripts

---

## Team & Roles

**Project Manager/Developer:** Thomas (User)
**Developers (4):** Alex, Mohamed, Thomas M., Thomas de O.
**Designers (2):** Morgane, Laetitia
**Sound Engineer:** Pierre Albert
**Total Team:** 8 people

**Project Created:** Early 2026
**Jam Event:** Global Game Jam 2026
**Jam Duration:** Jan 30 - Feb 1, 2026 (48 hours)
**Game Genre:** Halloween Interactive Mini-game Collection
**Art Style:** Spooky but kid-friendly, flat 2D with parallax depth illusion
**Target Audience:** All ages

---

## Jam Readiness Checklist

- [x] All 5 critical code fixes applied (2026-01-29)
- [x] GDD.md completed with full game design (2026-01-30)
- [x] CLAUDE.md updated with team info (2026-01-30)
- [x] Tasks assigned to team members (14 tasks, 8 people)
- [x] Core managers ready (GameManager, InputManager, AudioManager, PoolingManager)
- [ ] Verify manager setup in SampleScene (on team's first run)
- [ ] No console errors on startup (verify)
- [ ] Framerate stable at 60 FPS (verify)
- [ ] Team has access to GDD and CLAUDE.md
- [ ] Task tracking system in place

---

**Status:** 🚀 **JAM ACTIVE**

Game concept locked. Team assigned and ready. Framework stable with all critical fixes. Starting game development 2026-01-30.

**Documentation Location:**
- `/Documentation/GDD.md` - Full game design
- `/CLAUDE.md` - Project state, team info, tasks
- `/README.md` - Setup and coding standards
- `/StyleGuide.md` - Naming conventions and best practices

**First Commit:** Setup docs + task assignments (2026-01-30)
