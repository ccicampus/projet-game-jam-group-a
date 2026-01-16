using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

/// <summary>
/// Manages all audio playback including music and sound effects
/// Supports audio mixing and volume control with null checks and constants
/// </summary>
public class AudioManager : MonoBehaviour
{
    // Constants
    private const string MIXER_PARAM_MUSIC = "MusicVolume";
    private const string MIXER_PARAM_SFX = "SFXVolume";
    private const float MIN_DB = -80f;
    private const float DB_MULTIPLIER = 20f;

    public static class MusicTracks
    {
        public const string MAIN_MENU = "MainMenu";
        public const string GAMEPLAY = "Gameplay";
        public const string BOSS = "Boss";
    }

    // Singleton
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Music Clips")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip bossMusic;

    [Header("Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.7f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;

    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSources()
    {
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null)
        {
            Debug.LogWarning("Trying to play null music clip");
            return;
        }

        if (musicSource == null)
        {
            Debug.LogError("Music source not initialized");
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlayMusicByName(string musicName)
    {
        AudioClip clip = musicName switch
        {
            MusicTracks.MAIN_MENU => mainMenuMusic,
            MusicTracks.GAMEPLAY => gameplayMusic,
            MusicTracks.BOSS => bossMusic,
            _ => null
        };

        if (clip != null)
            PlayMusic(clip);
        else
            Debug.LogWarning($"Music '{musicName}' not found");
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource != null)
            musicSource.UnPause();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Trying to play null SFX clip");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogError("SFX source not initialized");
            return;
        }

        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlaySFXByName(string sfxName, float volume = 1f)
    {
        if (sfxClips.ContainsKey(sfxName))
        {
            PlaySFX(sfxClips[sfxName], volume);
        }
        else
        {
            Debug.LogWarning($"SFX '{sfxName}' not found");
        }
    }

    public void RegisterSFX(string name, AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning($"Attempted to register null audio clip: {name}");
            return;
        }

        if (!sfxClips.ContainsKey(name))
        {
            sfxClips.Add(name, clip);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        if (audioMixer != null)
        {
            float db = musicVolume > 0 ? Mathf.Log10(musicVolume) * DB_MULTIPLIER : MIN_DB;
            audioMixer.SetFloat(MIXER_PARAM_MUSIC, db);
        }
        else if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);

        if (audioMixer != null)
        {
            float db = sfxVolume > 0 ? Mathf.Log10(sfxVolume) * DB_MULTIPLIER : MIN_DB;
            audioMixer.SetFloat(MIXER_PARAM_SFX, db);
        }
        else if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            sfxClips.Clear();
        }
    }
}
