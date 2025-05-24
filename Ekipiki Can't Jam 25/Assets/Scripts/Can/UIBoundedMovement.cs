using UnityEngine;

public class UIAutoBounceWithCornerBounds : MonoBehaviour
{
    [Header("Corner Boundaries")]
    public RectTransform upperLeftBoundary;
    public RectTransform lowerLeftBoundary;
    public RectTransform upperRightBoundary;
    public RectTransform lowerRightBoundary;

    [Header("Movement Settings")]
    public Vector2 baseMoveSpeed = new Vector2(150f, 100f);
    private Vector2 direction = Vector2.one.normalized;
    private Vector2 currentMoveSpeed;

    [Header("Dash Settings")]
    public float dashSpeedMultiplier = 2f;
    public float dashDuration = 0.5f;
    public float minDashInterval = 2f;
    public float maxDashInterval = 5f;
    
    private float dashTimer = 0f;
    private float dashCooldown = 0f;
    private bool isDashing = false;

    private RectTransform uiElement;
    private float minX, maxX, minY, maxY;

    void Start()
    {
        uiElement = GetComponent<RectTransform>();
        if (uiElement == null)
        {
            Debug.LogError("This script requires a RectTransform component!");
            enabled = false;
            return;
        }

        currentMoveSpeed = baseMoveSpeed;
        ValidateBoundaries();
        CalculateBounds();
        ResetDashCooldown();
    }

    void Update()
    {
        HandleDash();
        MoveUIElement();
        CheckBoundaries();
    }

    void HandleDash()
    {
        if (!isDashing)
        {
            dashCooldown -= Time.deltaTime;
            if (dashCooldown <= 0f)
            {
                StartDash();
            }
        }
        else
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                EndDash();
            }
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        currentMoveSpeed = baseMoveSpeed * dashSpeedMultiplier;
        
        // Randomize direction slightly when dashing
        direction = new Vector2(
            Mathf.Clamp(direction.x + Random.Range(-0.5f, 0.5f), -1f, 1f),
            Mathf.Clamp(direction.y + Random.Range(-0.5f, 0.5f), -1f, 1f)
        ).normalized;
    }

    void EndDash()
    {
        isDashing = false;
        currentMoveSpeed = baseMoveSpeed;
        ResetDashCooldown();
    }

    void ResetDashCooldown()
    {
        dashCooldown = Random.Range(minDashInterval, maxDashInterval);
    }

    void MoveUIElement()
    {
        // Move the UI element
        uiElement.anchoredPosition += direction * currentMoveSpeed * Time.deltaTime;
    }

    void CheckBoundaries()
    {
        // Get current position
        Vector2 pos = uiElement.anchoredPosition;

        // Check horizontal boundaries
        if (pos.x < minX)
        {
            direction.x = Mathf.Abs(direction.x);
            pos.x = minX;
            if (isDashing) EndDash(); // Stop dash when hitting boundary
        }
        else if (pos.x > maxX)
        {
            direction.x = -Mathf.Abs(direction.x);
            pos.x = maxX;
            if (isDashing) EndDash(); // Stop dash when hitting boundary
        }

        // Check vertical boundaries
        if (pos.y < minY)
        {
            direction.y = Mathf.Abs(direction.y);
            pos.y = minY;
            if (isDashing) EndDash(); // Stop dash when hitting boundary
        }
        else if (pos.y > maxY)
        {
            direction.y = -Mathf.Abs(direction.y);
            pos.y = maxY;
            if (isDashing) EndDash(); // Stop dash when hitting boundary
        }

        uiElement.anchoredPosition = pos;
    }

    void ValidateBoundaries()
    {
        if (!upperLeftBoundary || !lowerLeftBoundary || !upperRightBoundary || !lowerRightBoundary)
        {
            Debug.LogError("All four boundary RectTransforms must be assigned!");
            enabled = false;
            return;
        }

        // Ensure boundaries form a proper rectangle
        if (upperLeftBoundary.anchoredPosition.x != lowerLeftBoundary.anchoredPosition.x ||
            upperRightBoundary.anchoredPosition.x != lowerRightBoundary.anchoredPosition.x ||
            upperLeftBoundary.anchoredPosition.y != upperRightBoundary.anchoredPosition.y ||
            lowerLeftBoundary.anchoredPosition.y != lowerRightBoundary.anchoredPosition.y)
        {
            Debug.LogWarning("Boundary positions don't form a perfect rectangle. Movement might be unexpected.");
        }
    }

    void CalculateBounds()
    {
        // Use left boundaries for minX, right for maxX
        minX = Mathf.Max(upperLeftBoundary.anchoredPosition.x, lowerLeftBoundary.anchoredPosition.x);
        maxX = Mathf.Min(upperRightBoundary.anchoredPosition.x, lowerRightBoundary.anchoredPosition.x);

        // Use lower boundaries for minY, upper for maxY
        minY = Mathf.Max(lowerLeftBoundary.anchoredPosition.y, lowerRightBoundary.anchoredPosition.y);
        maxY = Mathf.Min(upperLeftBoundary.anchoredPosition.y, upperRightBoundary.anchoredPosition.y);

        // Adjust for the moving element's own size and pivot
        Vector2 elementSize = uiElement.rect.size;
        minX += elementSize.x * uiElement.pivot.x;
        maxX -= elementSize.x * (1 - uiElement.pivot.x);
        minY += elementSize.y * uiElement.pivot.y;
        maxY -= elementSize.y * (1 - uiElement.pivot.y);
    }

    void OnDrawGizmosSelected()
    {
        if (upperLeftBoundary && lowerLeftBoundary && upperRightBoundary && lowerRightBoundary)
        {
            Gizmos.color = Color.green;
            Vector3 ul = upperLeftBoundary.position;
            Vector3 ur = upperRightBoundary.position;
            Vector3 ll = lowerLeftBoundary.position;
            Vector3 lr = lowerRightBoundary.position;

            Gizmos.DrawLine(ul, ur);
            Gizmos.DrawLine(ur, lr);
            Gizmos.DrawLine(lr, ll);
            Gizmos.DrawLine(ll, ul);
        }
    }
}