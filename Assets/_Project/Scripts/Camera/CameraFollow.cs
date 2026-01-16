using UnityEngine;

/// <summary>
/// Smooth camera following system with optional boundaries
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private bool autoFindPlayer = true;
    
    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    
    [Header("Look Ahead")]
    [SerializeField] private bool enableLookAhead = true;
    [SerializeField] private float lookAheadDistance = 2f;
    [SerializeField] private float lookAheadSpeed = 2f;
    
    [Header("Boundaries")]
    [SerializeField] private bool useBoundaries = false;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    
    [Header("Deadzone")]
    [SerializeField] private bool useDeadzone = false;
    [SerializeField] private Vector2 deadzoneSize = new Vector2(2f, 1f);
    
    private Vector3 currentVelocity;
    private float lookAheadX;
    
    private void Start()
    {
        if (autoFindPlayer && target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 targetPosition = target.position + offset;
        
        // Look ahead
        if (enableLookAhead)
        {
            float targetLookAhead = Mathf.Sign(target.localScale.x) * lookAheadDistance;
            lookAheadX = Mathf.Lerp(lookAheadX, targetLookAhead, lookAheadSpeed * Time.deltaTime);
            targetPosition.x += lookAheadX;
        }
        
        // Deadzone
        if (useDeadzone)
        {
            Vector3 currentPos = transform.position;
            float deadX = deadzoneSize.x / 2f;
            float deadY = deadzoneSize.y / 2f;
            
            if (Mathf.Abs(targetPosition.x - currentPos.x) < deadX)
                targetPosition.x = currentPos.x;
            
            if (Mathf.Abs(targetPosition.y - currentPos.y) < deadY)
                targetPosition.y = currentPos.y;
        }
        
        // Boundaries
        if (useBoundaries)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        }
        
        // Smooth follow
        Vector3 smoothedPosition = Vector3.SmoothDamp(
            transform.position, 
            targetPosition, 
            ref currentVelocity, 
            smoothSpeed
        );
        
        transform.position = smoothedPosition;
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw boundaries
        if (useBoundaries)
        {
            Gizmos.color = Color.yellow;
            Vector3 bottomLeft = new Vector3(minBounds.x, minBounds.y, 0);
            Vector3 topRight = new Vector3(maxBounds.x, maxBounds.y, 0);
            Vector3 size = topRight - bottomLeft;
            Gizmos.DrawWireCube(bottomLeft + size / 2f, size);
        }
        
        // Draw deadzone
        if (useDeadzone && target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(target.position + offset, new Vector3(deadzoneSize.x, deadzoneSize.y, 0));
        }
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    public void SetBoundaries(Vector2 min, Vector2 max)
    {
        minBounds = min;
        maxBounds = max;
        useBoundaries = true;
    }
}
