using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    public float scrollSpeed = 2f;
    private float spriteWidth;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (transform.position.x <= -spriteWidth)
        {
            transform.position += new Vector3(spriteWidth * 2f, 0, 0);
        }
    }
}
