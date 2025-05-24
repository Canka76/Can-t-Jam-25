using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;
    public GameObject imageDisplayUI;
    public GameObject backgroundPanel;

    [Header("Toggle Buttons")]
    public GameObject toggleOnButton;  // Button shown when image is active
    public GameObject toggleOffButton; // Button shown when image is inactive

    private Button[] allButtons;
    private bool isPaused = false;
    private bool isImageVisible = false;

    // Store original layout of the inactive button (assume toggleOffButton is default)
    private RectTransform toggleOffRect;
    private Vector2 initialAnchoredPosition;
    private Vector2 initialAnchorMin;
    private Vector2 initialAnchorMax;
    private Vector2 initialPivot;

    void Start()
    {
        allButtons = pauseMenuUI.GetComponentsInChildren<Button>(true);

        toggleOffRect = toggleOffButton?.GetComponent<RectTransform>();

        if (toggleOffRect != null)
        {
            initialAnchoredPosition = toggleOffRect.anchoredPosition;
            initialAnchorMin = toggleOffRect.anchorMin;
            initialAnchorMax = toggleOffRect.anchorMax;
            initialPivot = toggleOffRect.pivot;
        }

        UpdateToggleButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        backgroundPanel.SetActive(true);
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        backgroundPanel.SetActive(false);
        pauseMenuUI.SetActive(false);
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

        // Hide/show other pause buttons
        foreach (Button btn in allButtons)
        {
            if (btn.gameObject != toggleOnButton && btn.gameObject != toggleOffButton)
                btn.gameObject.SetActive(!isImageVisible);
        }

        // Activate/deactivate the right toggle buttons
        UpdateToggleButtons();

        // Move or reset toggle button
        if (toggleOffRect != null)
        {
            if (isImageVisible)
            {
                toggleOffRect.anchoredPosition = new Vector2(703, 341); // Target position
            }
            else
            {
                toggleOffRect.anchorMin = initialAnchorMin;
                toggleOffRect.anchorMax = initialAnchorMax;
                toggleOffRect.pivot = initialPivot;
                toggleOffRect.anchoredPosition = initialAnchoredPosition;
            }
        }
    }

    private void UpdateToggleButtons()
    {
        if (toggleOnButton != null)
            toggleOnButton.SetActive(isImageVisible);

        if (toggleOffButton != null)
            toggleOffButton.SetActive(!isImageVisible);
    }
}
