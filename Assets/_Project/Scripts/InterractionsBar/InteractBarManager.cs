#nullable enable
using UnityEngine;
using UnityEngine.Events;

public class InteractBarManager : MonoBehaviour
{
    private const int minStep = -1;
    private const int maxStep = 1;

    public static InteractBarManager? Instance;

    [Header("Debug")]
    [SerializeField] protected bool debugMode = false;

    public UnityEvent<int> OnInteract = new UnityEvent<int>();

    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(this.gameObject);
            Instance = null;
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance is null)
            return;

        Destroy(this.gameObject);
        Instance = null;
    }

    public void Increase()
    {
        this.OnInteract.Invoke(maxStep);
    }

    public void Decrease()
    {
        this.OnInteract.Invoke(minStep);
    }
}
