using UnityEngine;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Save/load system with async/await support and error handling
/// Supports JSON-based serialization with validation
/// </summary>
public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private string saveFilePath;
    private string backupFilePath;

    [System.Serializable]
    public class SaveData
    {
        public int currentLevel = 1;
        public int highScore = 0;
        public float musicVolume = 0.7f;
        public float sfxVolume = 1f;

        // Add more save data fields as needed
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            saveFilePath = Application.persistentDataPath + "/savegame.json";
            backupFilePath = Application.persistentDataPath + "/savegame.backup.json";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Synchronous save (for compatibility)
    /// </summary>
    public void SaveGame(SaveData data)
    {
        if (data == null)
        {
            Debug.LogError("Cannot save null save data");
            return;
        }

        try
        {
            string json = JsonUtility.ToJson(data, true);

            // Create backup of existing file
            if (File.Exists(saveFilePath))
            {
                File.Copy(saveFilePath, backupFilePath, overwrite: true);
            }

            File.WriteAllText(saveFilePath, json);
            Debug.Log($"Game saved to: {saveFilePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    /// <summary>
    /// Asynchronous save (recommended for no frame drops)
    /// </summary>
    public async Task SaveGameAsync(SaveData data)
    {
        if (data == null)
        {
            Debug.LogError("Cannot save null save data");
            return;
        }

        try
        {
            string json = JsonUtility.ToJson(data, true);

            // Create backup of existing file
            if (File.Exists(saveFilePath))
            {
                File.Copy(saveFilePath, backupFilePath, overwrite: true);
            }

            await File.WriteAllTextAsync(saveFilePath, json);
            Debug.Log($"Game saved to: {saveFilePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Async save failed: {e.Message}");
        }
    }

    /// <summary>
    /// Load save file with validation
    /// </summary>
    public SaveData LoadGame()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                if (data == null)
                {
                    Debug.LogWarning("Corrupted save file, trying backup");
                    return LoadBackup();
                }

                Debug.Log("Game loaded successfully");
                return data;
            }
            else
            {
                Debug.Log("No save file found, creating new save data");
                return new SaveData();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}. Trying backup...");
            return LoadBackup();
        }
    }

    /// <summary>
    /// Load from backup file
    /// </summary>
    private SaveData LoadBackup()
    {
        try
        {
            if (File.Exists(backupFilePath))
            {
                string json = File.ReadAllText(backupFilePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                if (data != null)
                {
                    Debug.Log("Backup loaded successfully");
                    return data;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Backup load failed: {e.Message}");
        }

        Debug.LogWarning("No valid backup found, returning default save data");
        return new SaveData();
    }

    public bool SaveExists()
    {
        return File.Exists(saveFilePath);
    }

    public void DeleteSave()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                Debug.Log("Save file deleted");
            }

            if (File.Exists(backupFilePath))
            {
                File.Delete(backupFilePath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to delete save: {e.Message}");
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
