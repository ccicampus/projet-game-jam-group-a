# Game Design Document

**Last Updated:** 2026-01-30
**Status:** In Development (Jam Concept - Subject to Change)

---

## Game Overview

**Title:** The Masked Visitor (Working Title)
**Theme:** Global Game Jam 2026 - "Mask"
**Genre:** Halloween Interactive Mini-game Collection
**Target Platform:** PC/Console
**Target Audience:** All ages (kid-friendly, spooky but not scary)
**Duration:** Single-session gameplay (loop-based)

---

## Core Gameplay

### Setting
- **Scene:** A single front door of a house
- **Perspective:** Side-view or isometric with parallax depth illusion (2D)
- **Character:** An old person doing trick-or-treat on Halloween
- **Technical Approach:** Layered 2D sprites with parallax scrolling to simulate movement through hallway

### Main Loop
1. **Cinematic Approach:** Walk through hallway toward front door (3D cinematic animation)
2. **Door Opens:** Encounter a masked visitor
3. **Clue-Gathering Mini-games:** Play 2-3 random mini-games to assess the visitor
4. **Judgment Phase:** Decide if they're "Nice" or "Monster" (with timer pressure)
5. **Resolution:** Fade out/reset
6. **Repeat:** Next visitor appears

### Mini-Games (Clue Providers)

| Mini-Game | Mechanic | Purpose |
|-----------|----------|---------|
| **Combat Training** | Grab cane/umbrella, QTE combat | Assess visitor's threat level |
| **Quick-Time Events** | Button timing | Reflects character's readiness |
| **Point & Click Examine** | Click hotspots on visitor | Gather visual clues |
| **Candy Basket Selection** | Choose candies, avoid spoilt ones | Personality assessment (risk tolerance) |
| **Memory Game** | Match pairs (memory fitting old character) | Trust-building mechanic |

### Judgment Mechanic
- Mini-game results = score/clues about visitor
- Timer counts down for final decision
- Choose "Nice" or "Monster"
- Score increases on correct judgments
- Malus on wrong judgments (spoilt candy mechanic)

### Objectives & Win Condition
- **Primary:** Maximize score by correctly judging visitors
- **Secondary:** Awareness element (recognize bias/stereotyping through masked strangers)
- **Progression:** Continuous loop - game ends when player decides (high-score focus)

---

## Art Style

- **Visual Approach:** Flat 2D art + 3D cinematic hallway/door sequence
- **Tone:** Spooky but whimsical, burlesque/quirky Halloween energy
- **Art Style:** Stylized, not realistic (cartoonish masks, exaggerated expressions)
- **Color Palette:** Oranges, purples, blacks (Halloween), with warm accent colors for nice characters
- **Animation Style:** Smooth cinematic walking, expressive character animations, simple mask transitions

### Scene Design
- 3D hallway: Warm lighting, slightly eerie but inviting
- Door opening animation: Smooth, cinematic (emphasize anticipation)
- Masks: Varied (fun, spooky, unusual) - visual variety between visitors

---

## Audio

### Music
- **Hallway/Main Theme:** Ambient, Halloween-themed (spooky but playful)
- **Mini-game Themes:** Upbeat, quirky, fits each game type
- **Decision Timer:** Tension-building music (countdown effect)
- **Loop Reset:** Brief palette-cleanser audio cue

### Sound Effects
- Door creak/open
- Footsteps (old character - slightly slower pace)
- Mini-game interactions (button presses, clicks, candy sounds)
- Timer ticking
- Success/fail audio cues
- Visitor voice lines/reactions (optional)

---

## Technical Specifications

- **Unity Version:** 6.2 (6000.3.2f1)
- **Resolution:** 1920x1080 (16:9)
- **Frame Rate:** 60 FPS
- **Graphics:** 2D sprites with parallax layering (hallway + UI)
- **Control Scheme:** Keyboard (WASD/Arrows) + Gamepad support
- **Physics:** Minimal (stateful/turn-based, not physics-heavy)

### Core Systems Needed
- **Mini-Game Manager:** Randomizes and runs mini-games
- **Judgment System:** Tracks decisions and calculates score
- **Timer System:** Countdown for judgment phase
- **Scene Manager:** Hallway animation + door interaction
- **Audio Manager:** (Provided by starter framework)
- **UI Manager:** Score, timer, decision buttons

---

## Known Constraints & Notes

1. **Scope:** Single door, looping encounters (very manageable for 48-hour jam)
2. **Mini-games:** Alex building combat game; others TBD
3. **Narrative:** Minimal story (old person, Halloween setting, implicit moral questions)
4. **End Condition:** TBD (high score? Time limit? Player chooses to stop?)
5. **Awareness Element:** Optional layer - game can subtly encourage questioning stereotypes

---

## Next Steps (Immediate)

- [ ] Task assignments to team (5 devs)
- [ ] Create 3D hallway + door scene
- [ ] Build mini-game framework
- [ ] Design visitor character variations
- [ ] Implement judgment/score system
