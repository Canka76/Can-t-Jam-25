using UnityEngine;

public class ConstrainedBetweenTwo : MonoBehaviour
{
    public Transform pointA;  // First boundary object
    public Transform pointB;  // Second boundary object
    
    void Update()
    {
        // Constrain position to the line between pointA and pointB
        transform.position = GetClosestPointOnLine(transform.position);
    }
    
    // Returns the closest point on the line segment between pointA and pointB
    Vector3 GetClosestPointOnLine(Vector3 currentPosition)
    {
        Vector3 lineDirection = pointB.position - pointA.position;
        float lineLength = lineDirection.magnitude;
        Vector3 normalizedDirection = lineDirection.normalized;
        
        Vector3 pointRelativeToA = currentPosition - pointA.position;
        float projection = Vector3.Dot(pointRelativeToA, normalizedDirection);
        
        // Clamp the projection to stay between the two points
        projection = Mathf.Clamp(projection, 0f, lineLength);
        
        return pointA.position + normalizedDirection * projection;
    }
}