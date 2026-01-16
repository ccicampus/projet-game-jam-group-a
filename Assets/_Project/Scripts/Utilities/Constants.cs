/// <summary>
/// Game-wide constants and configuration values
/// </summary>
public static class Constants
{
    // Scene names
    public const string SCENE_MAIN_MENU = "_Main";
    public const string SCENE_LEVEL_01 = "Level_01";
    
    // Tags
    public const string TAG_PLAYER = "Player";
    public const string TAG_ENEMY = "Enemy";
    public const string TAG_GROUND = "Ground";
    public const string TAG_COLLECTIBLE = "Collectible";
    
    // Layers
    public const int LAYER_GROUND = 6;
    public const int LAYER_PLAYER = 7;
    public const int LAYER_ENEMY = 8;
    public const int LAYER_PROJECTILE = 9;
    public const int LAYER_ITEMS = 10;
    
    // Animation Parameters
    public const string ANIM_SPEED = "Speed";
    public const string ANIM_IS_GROUNDED = "IsGrounded";
    public const string ANIM_JUMP = "Jump";
    public const string ANIM_ATTACK = "Attack";
    public const string ANIM_HURT = "Hurt";
    public const string ANIM_DEATH = "Death";
    
    // Audio Mixer Parameters
    public const string MIXER_MUSIC_VOLUME = "MusicVolume";
    public const string MIXER_SFX_VOLUME = "SFXVolume";
    public const string MIXER_MASTER_VOLUME = "MasterVolume";
    
    // PlayerPrefs Keys
    public const string PREF_MUSIC_VOLUME = "MusicVolume";
    public const string PREF_SFX_VOLUME = "SFXVolume";
    public const string PREF_HIGH_SCORE = "HighScore";
    public const string PREF_CURRENT_LEVEL = "CurrentLevel";
    
    // Game Balance
    public const int MAX_PLAYER_HEALTH = 100;
    public const int MAX_ENEMY_SPAWNS = 50;
    public const float RESPAWN_TIME = 3f;
}
