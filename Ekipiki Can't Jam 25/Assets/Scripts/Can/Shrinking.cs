using UnityEngine;

public class Shrinking : MonoBehaviour
{
    public KeyCode increaseKey = KeyCode.Space;

    [Header("Scale Increase")]
    public float minScaleIncrease = 0.2f;
    public float maxScaleIncrease = 0.5f;

    [Header("Shrink Settings")]
    public float minShrink = 0.2f;
    public float maxShrink = 0.4f;
    public float shrinkInterval = 0.5f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        InvokeRepeating(nameof(ShrinkScale), shrinkInterval, shrinkInterval);
    }

    void Update()
    {
        if (Input.GetKeyDown(increaseKey))
        {
            float scaleIncrease = Random.Range(minScaleIncrease, maxScaleIncrease);
            transform.localScale += Vector3.one * scaleIncrease;
        }
    }

    void ShrinkScale()
    {
        float shrinkAmount = Random.Range(minShrink, maxShrink);

        // Double shrink if any axis is >= 2
        if (transform.localScale.x >= 2f || transform.localScale.y >= 2f || transform.localScale.z >= 2f)
        {
            shrinkAmount *= 2f;
        }

        Vector3 newScale = transform.localScale - Vector3.one * shrinkAmount;

        // Clamp to original scale to prevent disappearing
        newScale = Vector3.Max(newScale, originalScale);
        transform.localScale = newScale;
    }
}