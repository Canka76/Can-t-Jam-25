using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public Transform player;
    private float speed; 

    void Start()
    {
        speed = Random.Range(6f, 8f); 
        Destroy(gameObject, 10f);     
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.position += (Vector3)(dir * speed * Time.deltaTime);
        }
    }
}
