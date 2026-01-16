using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Object pooling system to improve performance by reusing GameObjects
/// IMPORTANT: Objects are only added back to the pool when ReturnToPool() is called
/// Do NOT immediately enqueue objects after spawning (common bug that was fixed here)
/// </summary>
public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        [SerializeField] private string tag;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int size;

        public string Tag => tag;
        public GameObject Prefab => prefab;
        public int Size => size;
    }

    [Header("Pools")]
    [SerializeField] private List<Pool> pools = new List<Pool>();

    [Header("Settings")]
    [SerializeField] private bool expandPool = true;
    [SerializeField] private Transform poolParent;

    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, GameObject> poolPrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        if (poolParent == null)
        {
            GameObject parentObj = new GameObject("PooledObjects");
            poolParent = parentObj.transform;
            poolParent.SetParent(transform);
        }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        poolPrefabs = new Dictionary<string, GameObject>();

        foreach (Pool pool in pools)
        {
            if (pool.Prefab == null)
            {
                Debug.LogError($"Pool '{pool.Tag}' has a null prefab");
                continue;
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = CreatePooledObject(pool.Prefab);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.Tag, objectPool);
            poolPrefabs.Add(pool.Tag, pool.Prefab);
        }
    }

    private GameObject CreatePooledObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, poolParent);
        obj.SetActive(false);
        return obj;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag '{tag}' doesn't exist.");
            return null;
        }

        GameObject objectToSpawn;

        if (poolDictionary[tag].Count == 0 && expandPool)
        {
            objectToSpawn = CreatePooledObject(poolPrefabs[tag]);
        }
        else if (poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning($"Pool '{tag}' is empty and expansion is disabled.");
            return null;
        }
        else
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // CRITICAL BUG FIX: Do NOT enqueue the object back here!
        // Objects should only be enqueued in ReturnToPool() after they're done being used.
        // The old code immediately returned objects to the pool, causing multiple systems
        // to reference the same object at once.

        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag '{tag}' doesn't exist.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(poolParent);
        // Return object to the pool queue only when it's done being used
        poolDictionary[tag].Enqueue(obj);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;

            // Clean up pooled objects
            if (poolDictionary != null)
            {
                foreach (var pool in poolDictionary.Values)
                {
                    while (pool.Count > 0)
                    {
                        GameObject obj = pool.Dequeue();
                        if (obj != null) Destroy(obj);
                    }
                }
                poolDictionary.Clear();
            }

            if (poolPrefabs != null)
            {
                poolPrefabs.Clear();
            }
        }
    }
}

/*
USAGE EXAMPLE WITH Ipoolable INTERFACE (optional):

public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
}

public class Bullet : MonoBehaviour, IPoolable
{
    public void OnSpawn()
    {
        // Reset bullet state when spawned
    }

    public void OnDespawn()
    {
        // Cleanup when returning to pool
    }
}

// In your game code:
GameObject bullet = PoolingManager.Instance.SpawnFromPool("Bullet", firePosition, rotation);

// When done with the object:
PoolingManager.Instance.ReturnToPool("Bullet", bullet);
*/
