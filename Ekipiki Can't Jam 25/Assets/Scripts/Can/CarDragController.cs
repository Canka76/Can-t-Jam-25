using UnityEngine;

public class CarDragController : MonoBehaviour
{
    [SerializeField] private Collider2D roadCollider;
    [SerializeField] private float returnForce = 10f;
    [SerializeField] private float maxForceDistance = 2f;
    
    private bool isDragging = false;
    private Rigidbody2D rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnMouseDown()
    {
        isDragging = true;
        rb.linearVelocity = Vector2.zero;
    }
    
    private void OnMouseUp()
    {
        isDragging = false;
    }
    
    private void Update()
    {
        if (isDragging)
        {
            // Convert mouse position to world position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePos);
        }
    }
    
    private void FixedUpdate()
    {
        if (!roadCollider.OverlapPoint(transform.position))
        {
            // Car is off the road - apply return force
            Vector2 closestRoadPoint = roadCollider.ClosestPoint(transform.position);
            Vector2 direction = (closestRoadPoint - (Vector2)transform.position).normalized;
            
            float distance = Vector2.Distance(transform.position, closestRoadPoint);
            float forceMultiplier = Mathf.Clamp01(distance / maxForceDistance);
            
            rb.AddForce(direction * returnForce * forceMultiplier, ForceMode2D.Force);
        }
    }
}