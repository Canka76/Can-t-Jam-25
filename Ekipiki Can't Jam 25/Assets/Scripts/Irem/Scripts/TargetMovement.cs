using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public Transform player;
    public float speed = 8f;
    void Start()
    {
        Destroy(gameObject, 10f); 
    }
    void Update()
    {
        if (player != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * speed * Time.deltaTime);
        }
    }
}
