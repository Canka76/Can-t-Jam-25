using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private int progress = 0;
    public int currentProgress = 0;
    public int currentPuzzleQueue = 0;

    public GameObject[] puzzles;

    void Start()
    {
        currentProgress = progress;
    }

    void Update()
    {
        if (currentProgress != progress)
        {
            ProgressChanged();
        }
    }

    void ProgressChanged()
    {
        if (currentProgress > -2 && currentProgress < 2)
        {
            Debug.Log($"Default state: {currentProgress}");
            progress = currentProgress;
        }
        else if (currentProgress < -2 && currentProgress > -5)
        {
            Debug.Log($"Enemy is winning state: {currentProgress}");
            progress = currentProgress;
        }
        else if (currentProgress > 2 && currentProgress < 5)
        {
            Debug.Log($"Player is winning state: {currentProgress}");
            progress = currentProgress;
        }
        else if (currentProgress >= 5)
        {
            Debug.LogWarning($"Enemy won: {currentProgress}");
            progress = currentProgress;
        }
        else if (currentProgress <= -5)
        {
            Debug.LogWarning($"Enemy won: {currentProgress}");
            progress = currentProgress;
        }
        
    }
    
}

