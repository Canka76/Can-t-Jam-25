using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class QuickTimeEvent : MonoBehaviour
{
    public TextMeshProUGUI qteText; // Assign in Inspector
    public float timeLimit = 3f;
    public PuzzleManager puzzleManager;

    private KeyCode[] possibleKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    private KeyCode currentKey;
    private bool inputReceived = false;

    void Start()
    {
        StartCoroutine(StartQTE());
    }

    IEnumerator StartQTE()
    {
        // Pick a random key
        currentKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
        qteText.text = $"PRESS: {currentKey}";
        inputReceived = false;

        float timer = 0f;
        while (timer < timeLimit && !inputReceived)
        {
            if (Input.GetKeyDown(currentKey))
            {
                inputReceived = true;
                qteText.text = "Success!";
                puzzleManager.currentProgress++;
                Debug.Log(puzzleManager.currentProgress);
                break;
            }
            else if (AnyWrongKeyPressed())
            {
                inputReceived = true;
                qteText.text = "Wrong Key!";
                puzzleManager.currentProgress--;
                Debug.Log(puzzleManager.currentProgress);
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (!inputReceived)
        {
            qteText.text = "Too Late!";
        }

        // Wait a bit before starting again
        yield return new WaitForSeconds(2f);
        StartCoroutine(StartQTE());
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