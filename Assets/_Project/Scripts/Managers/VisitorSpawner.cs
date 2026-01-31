using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages visitor spawning and tracking
/// Singleton pattern matching other managers
/// </summary>
public class VisitorSpawner : MonoBehaviour
{
    public static VisitorSpawner Instance { get; private set; }

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject tutorialVisitorPrefab;
    [SerializeField] private GameObject[] visitorPrefabs;

    [Header("Tracking")]
    [SerializeField] private List<string> encounteredVisitorNames = new List<string>();

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    private Visitor currentVisitor;
    private int visitorCount = 0;

    private void Awake()
    {
        // Singleton pattern
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

    /// <summary>
    /// Spawn next visitor - tutorial on first round, random after
    /// </summary>
    public Visitor SpawnNextVisitor()
    {
        if (visitorCount == 0 && tutorialVisitorPrefab != null)
        {
            // First visitor - spawn tutorial vampire
            if (debugMode)
                Debug.Log("Spawning tutorial visitor (vampire)");
            return SpawnVisitor(tutorialVisitorPrefab);
        }
        else
        {
            // Subsequent visitors - random from pool
            return SpawnRandomVisitor();
        }
    }

    /// <summary>
    /// Spawn a random visitor at the door
    /// </summary>
    public Visitor SpawnRandomVisitor()
    {
        if (visitorPrefabs == null || visitorPrefabs.Length == 0)
        {
            Debug.LogError("No visitor prefabs assigned to VisitorSpawner!");
            return null;
        }

        // Pick random prefab
        GameObject randomPrefab = visitorPrefabs[Random.Range(0, visitorPrefabs.Length)];

        return SpawnVisitor(randomPrefab);
    }

    /// <summary>
    /// Spawn a specific visitor prefab
    /// </summary>
    public Visitor SpawnVisitor(GameObject visitorPrefab)
    {
        if (visitorPrefab == null)
        {
            Debug.LogError("Visitor prefab is null!");
            return null;
        }

        // Clean up previous visitor if exists
        if (currentVisitor != null)
        {
            Destroy(currentVisitor.gameObject);
        }

        // Instantiate at spawn point
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        GameObject visitorGO = Instantiate(visitorPrefab, spawnPos, Quaternion.identity);

        currentVisitor = visitorGO.GetComponent<Visitor>();

        if (currentVisitor == null)
        {
            Debug.LogError("Spawned prefab does not have Visitor component!");
            Destroy(visitorGO);
            return null;
        }

        // Track encounter
        string visitorName = currentVisitor.VisitorName;
        if (!encounteredVisitorNames.Contains(visitorName))
        {
            encounteredVisitorNames.Add(visitorName);
        }

        visitorCount++;

        if (debugMode)
            Debug.Log($"Spawned visitor #{visitorCount}: {visitorName} at {spawnPos}");

        // Trigger appearance
        currentVisitor.Appear();

        return currentVisitor;
    }

    /// <summary>
    /// Get the current active visitor
    /// </summary>
    public Visitor GetCurrentVisitor()
    {
        return currentVisitor;
    }

    /// <summary>
    /// Check if a visitor has been encountered before
    /// </summary>
    public bool HasEncountered(string visitorName)
    {
        return encounteredVisitorNames.Contains(visitorName);
    }

    /// <summary>
    /// Get total unique visitors encountered
    /// </summary>
    public int GetEncounterCount()
    {
        return encounteredVisitorNames.Count;
    }

    /// <summary>
    /// Get total visitors spawned
    /// </summary>
    public int GetVisitorCount()
    {
        return visitorCount;
    }

    /// <summary>
    /// Clear encounter history (for new game)
    /// </summary>
    public void ClearEncounterHistory()
    {
        encounteredVisitorNames.Clear();
        visitorCount = 0;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
