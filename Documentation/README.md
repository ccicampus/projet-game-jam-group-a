# Unity 2D Game Project

## Project Structure

This project follows a modular folder structure designed for team collaboration.

### Main Folders

- **_Project/**: Main project folder (underscore keeps it at top of Assets)
  - **Scenes/**: All game scenes
  - **Scripts/**: All C# scripts organized by category
  - **Prefabs/**: Reusable game objects
  - **Sprites/**: All 2D artwork
  - **Audio/**: Music and sound effects
  - **Animations/**: Animation controllers and clips
  - **Materials/**: Sprite materials and shaders
  - **Fonts/**: Text fonts
  - **Data/**: ScriptableObjects and configuration data

### Coding Standards

- Use meaningful variable names
- Comment complex logic
- Follow C# naming conventions (PascalCase for public, camelCase for private)
- Keep scripts focused on single responsibility
- Use [SerializeField] instead of public fields when possible

### Git Workflow

1. Always pull before starting work
2. Create feature branches for new features
3. Commit often with clear messages
4. Never commit generated files (Library/, Temp/, etc.)

### Audio Setup

- Music should be imported as compressed (Vorbis)
- SFX should be imported as PCM for short sounds
- Use the AudioMixer for volume control

### Sprite Import Settings

- Set Pixels Per Unit consistently (usually 16 or 32 for pixel art)
- Use Point (no filter) for pixel art
- Use Bilinear for smooth art
- Set appropriate Sprite Mode (Single/Multiple)

## Getting Started

1. Open the project in Unity 6.2
2. Run the C# script generation scripts to create starter code
3. Check ProjectSettings for proper layer and tag setup
4. Review the scene setup in Scenes/_Main.unity
