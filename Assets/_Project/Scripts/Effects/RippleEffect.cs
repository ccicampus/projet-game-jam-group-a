using UnityEngine;
using System.Collections;

/// <summary>
/// Circle ripple effect - expands outward and fades
/// </summary>
public class RippleEffect : MonoBehaviour
{
    public static void PlayRipple(Vector3 position, Sprite circlePNG, float duration = 0.5f)
    {
        // Create temporary GameObject
        GameObject ripple = new GameObject("Ripple");
        ripple.transform.position = position;

        // Add SpriteRenderer
        SpriteRenderer spriteRenderer = ripple.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = circlePNG;
        spriteRenderer.sortingOrder = 10; // On top

        // Start animation
        ripple.AddComponent<RippleEffect>().StartRipple(duration);
    }

    private void StartRipple(float duration)
    {
        StartCoroutine(AnimateRipple(duration));
    }

    private IEnumerator AnimateRipple(float duration)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            // Scale outward (start small, grow large)
            float scale = Mathf.Lerp(0.5f, 2.5f, progress);
            transform.localScale = new Vector3(scale, scale, 1f);

            // Fade out (alpha 1 → 0)
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0f, progress);
            spriteRenderer.color = color;

            yield return null;
        }

        // Clean up
        Destroy(gameObject);
    }
}
