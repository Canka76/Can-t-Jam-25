using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject imageDisplayUI;         // The panel/image to show,
    public GameObject imageUI;
    public Button toggleImageButton;          // Reference to the Toggle button
    public RectTransform toggleButtonRect;    // RectTransform of the Toggle button
    public Sprite normalSprite;                // Default appearance
    public Sprite activeSprite;                // Active appearance

    private Button[] allButtons;               // All buttons in the pause menu
    private bool isPaused = false;
    private bool isImageVisible = false;

    // Store initial values
    private Vector2 initialAnchoredPosition;
    private Vector2 initialAnchorMin;
    private Vector2 initialAnchorMax;
    private Vector2 initialPivot;

    void Start()
    {
        allButtons = pauseMenuUI.GetComponentsInChildren<Button>(true);

        if (toggleButtonRect != null)
        {
            initialAnchoredPosition = toggleButtonRect.anchoredPosition;
            initialAnchorMin = toggleButtonRect.anchorMin;
            initialAnchorMax = toggleButtonRect.anchorMax;
            initialPivot = toggleButtonRect.pivot;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleImageDisplay()
    {
        isImageVisible = !isImageVisible;

        imageDisplayUI.SetActive(isImageVisible);

        // Deactivate or activate all buttons except the toggle button
        foreach (Button btn in allButtons)
        {
            if (btn != toggleImageButton)
                btn.gameObject.SetActive(!isImageVisible);
        }

        // Change the appearance
        if (toggleImageButton != null && toggleImageButton.image != null)
        {
            toggleImageButton.image.sprite = isImageVisible ? activeSprite : normalSprite;
        }

        // Move toggle button to specified position or back to initial
        if (toggleButtonRect != null)
        {
            if (isImageVisible)
            {
                toggleButtonRect.anchoredPosition = new Vector2(-578, 294);
                
                

            }
            else
            {
                toggleButtonRect.anchorMin = initialAnchorMin;
                toggleButtonRect.anchorMax = initialAnchorMax;
                toggleButtonRect.pivot = initialPivot;
                toggleButtonRect.anchoredPosition = initialAnchoredPosition;
            }
        }
    }
}
