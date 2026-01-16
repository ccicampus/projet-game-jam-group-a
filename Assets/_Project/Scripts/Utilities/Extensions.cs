using UnityEngine;

/// <summary>
/// Useful extension methods for common Unity types
/// </summary>
public static class Extensions
{
    // Vector2 Extensions
    public static Vector2 With(this Vector2 vector, float? x = null, float? y = null)
    {
        return new Vector2(x ?? vector.x, y ?? vector.y);
    }
    
    public static Vector2 Add(this Vector2 vector, float? x = null, float? y = null)
    {
        return new Vector2(vector.x + (x ?? 0), vector.y + (y ?? 0));
    }
    
    // Vector3 Extensions
    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }
    
    public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(vector.x + (x ?? 0), vector.y + (y ?? 0), vector.z + (z ?? 0));
    }
    
    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }
    
    // Transform Extensions
    public static void ResetTransform(this Transform transform)
    {
        transform.position = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
    
    public static void SetX(this Transform transform, float x)
    {
        Vector3 pos = transform.position;
        pos.x = x;
        transform.position = pos;
    }
    
    public static void SetY(this Transform transform, float y)
    {
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }
    
    public static void SetZ(this Transform transform, float z)
    {
        Vector3 pos = transform.position;
        pos.z = z;
        transform.position = pos;
    }
    
    // Rigidbody2D Extensions
    public static void SetVelocityX(this Rigidbody2D rb, float x)
    {
        rb.linearVelocity = new Vector2(x, rb.linearVelocity.y);
    }
    
    public static void SetVelocityY(this Rigidbody2D rb, float y)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, y);
    }
    
    // GameObject Extensions
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }
    
    // Color Extensions
    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    
    // Float Extensions
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
