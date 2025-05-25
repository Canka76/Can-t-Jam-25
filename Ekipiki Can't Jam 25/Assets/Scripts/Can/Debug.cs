using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public GameObject goal;
    [SerializeField] private RectTransform cursorTransform;

    private void OnEnable()
    {
        cursorTransform.anchoredPosition = new Vector2(-170f, 0f);
        if (goal != null)
        {
            Image img = goal.GetComponent<Image>();
            if (img != null)
            {
                img.enabled = true; // Disable the Image component rendering
            }
            else
            {
                Debug.LogWarning("No Image component found on target GameObject.");
            }
        }
    }
}
