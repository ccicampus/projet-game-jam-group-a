# Code & Art Style Guide

## C# Coding Standards

### Naming Conventions
\`\`\`csharp
// Classes: PascalCase
public class PlayerController { }

// Public fields/properties: PascalCase
public int MaxHealth { get; set; }

// Private fields: camelCase with underscore prefix
private int _currentHealth;

// Serialized fields: camelCase
[SerializeField] private float moveSpeed;

// Methods: PascalCase
public void TakeDamage(int amount) { }

// Local variables: camelCase
int damageAmount = 10;

// Constants: UPPER_CASE
private const int MAX_ENEMIES = 50;
\`\`\`

### Script Organization
See README.md for detailed organization guidelines.

### Best Practices
- Use [SerializeField] instead of public for inspector fields
- Cache component references in Awake()
- Avoid Update() when possible, use events
- Use object pooling for frequently spawned objects
- Prefer composition over inheritance
- Use meaningful variable names (avoid single letters except for loops)

## Art Style Guidelines

### Sprite Guidelines
- Consistent pixel density across all sprites
- Maintain consistent outline thickness
- Use limited color palette
- Keep character proportions consistent

### Animation Guidelines
- Maintain consistent frame timing
- Use anticipation and follow-through
- Add squash and stretch for impact

### UI Guidelines
- Consistent button styles
- Clear visual hierarchy
- Readable fonts with good contrast
- Responsive feedback for all interactions
