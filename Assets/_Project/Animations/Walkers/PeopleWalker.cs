using UnityEngine;

public class PeopleWalker : MonoBehaviour
{
    public float speed = 3f;
    public bool walkRight = true;
    public float startDelay = 0f;
    public float walkDistance = 5f;

    private float elapsedTime = 0f;
    private float direction = 1f;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        direction = walkRight ? 1f : -1f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < startDelay)
            return;

        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        // Check if reached distance, turn around                                                 
        if (Mathf.Abs(transform.position.x - startPos.x) >= walkDistance)
        {
            direction *= -1f;

            // Flip sprite on Y axis                                                              
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }
}