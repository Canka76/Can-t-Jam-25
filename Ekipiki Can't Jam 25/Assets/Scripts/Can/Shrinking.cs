using System.Collections;
using UnityEngine;

public class Shrinking : MonoBehaviour
{
    public KeyCode increaseKey = KeyCode.Space;
    public PuzzleManager puzzleManager;

    [Header("Scale Increase")]
    public float minScaleIncrease = 0.2f;
    public float maxScaleIncrease = 0.5f;

    [Header("Shrink Settings")]
    public float minShrink = 0.2f;
    public float maxShrink = 0.4f;
    public float shrinkInterval = 0.5f;

    private Vector3 originalScale;
    private bool hasGrown = false;
    private bool hasShrunk = false;
    private bool isInitialized = false;

    void OnEnable()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(2f);

        originalScale = transform.localScale;
        InvokeRepeating(nameof(ShrinkScale), shrinkInterval, shrinkInterval);
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized) return;

        if (Input.GetKeyDown(increaseKey))
        {
            float scaleIncrease = Random.Range(minScaleIncrease, maxScaleIncrease);
            transform.localScale += Vector3.one * scaleIncrease;
            hasShrunk = false; // Reset shrink flag when growing
        }

        if (transform.localScale.x > 2f && !hasGrown)
        {
            puzzleManager.currentProgress += 2;
            hasGrown = true;
            hasShrunk = false;
            Debug.LogWarning("Scale > 2");

            StartCoroutine(ResetScale());
        }

        if (transform.localScale.x <= originalScale.x / 2 && !hasShrunk)
        {
            puzzleManager.currentProgress -= 2;
            hasShrunk = true;
            hasGrown = false;
            Debug.LogWarning("Scale <= original");

            StartCoroutine(ResetScale());
        }
    }

    private IEnumerator ResetScale()
    {
        yield return new WaitForSeconds(1f);
        transform.localScale = originalScale;
        hasGrown = false;
        hasShrunk = false;
    }

    void ShrinkScale()
    {
        float shrinkAmount = Random.Range(minShrink, maxShrink);

        if (transform.localScale.x >= 2f || transform.localScale.y >= 2f || transform.localScale.z >= 2f)
        {
            shrinkAmount *= 2f;
        }

        Vector3 newScale = transform.localScale - Vector3.one * shrinkAmount;

        // Clamp each axis to a minimum of 0.3
        newScale.x = Mathf.Max(newScale.x, 0.3f);
        newScale.y = Mathf.Max(newScale.y, 0.3f);
        newScale.z = Mathf.Max(newScale.z, 0.3f);

        transform.localScale = newScale;
    }
}
