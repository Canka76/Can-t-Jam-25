using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Random = UnityEngine.Random;

public class QuickTimeEvent : MonoBehaviour
{
    public TextMeshProUGUI qteText; // Assign in Inspector
    public float timeLimit = 3f;
    public PuzzleManager puzzleManager;
    public Image duvar;

    private KeyCode[] possibleKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    private KeyCode currentKey;
    private bool inputReceived = false;

    void Start()
    {
        StartCoroutine(StartQTE());
    }

    private void OnEnable()
    {
        StartCoroutine(StartQTE());
    }

    IEnumerator StartQTE()
    {
        // Pick a random key
        currentKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
        qteText.text = currentKey.ToString();
        qteText.color = Color.white; // Reset text color at start
        inputReceived = false;

        float timer = 0f;
        while (timer < timeLimit && !inputReceived)
        {
            if (Input.GetKeyDown(currentKey))
            {
                inputReceived = true;
                qteText.text = "Success!";
                puzzleManager.currentProgress++;
                Debug.Log($"Progress increased: {puzzleManager.currentProgress}");
                break;
            }
            else if (AnyWrongKeyPressed())
            {
                inputReceived = true;
                qteText.text = "Wrong Key!";
                puzzleManager.currentProgress--;
                Debug.Log($"Progress decreased: {puzzleManager.currentProgress}");
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (!inputReceived)
        {
            qteText.text = "Too Late!";
            puzzleManager.currentProgress--;
            Debug.Log($"Progress decreased (timeout): {puzzleManager.currentProgress}");
        }

        // Determine success status for color change
        bool success = qteText.text == "Success!";

        // Change text color based on success or failure
        yield return StartCoroutine(ChangeTextColor(success));

        // Restart the QTE
        StartCoroutine(StartQTE());
    }

    IEnumerator ChangeTextColor(bool success)
    {
        duvar.color = success
            ? new Color(146f/255f, 206f/255f, 87f/255f)
            : new Color(183f/255f, 80f/255f, 80f/255f);

        // Wait for 2 seconds before resetting color
        yield return new WaitForSeconds(2f);

        duvar.color = Color.white;
    }

    bool AnyWrongKeyPressed()
    {
        foreach (KeyCode key in possibleKeys)
        {
            if (key != currentKey && Input.GetKeyDown(key))
                return true;
        }
        return false;
    }
}
