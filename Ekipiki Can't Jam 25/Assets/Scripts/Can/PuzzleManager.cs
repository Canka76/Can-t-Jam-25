using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private int progress = 0;
    public int currentProgress = 0;

    private int currentPuzzleQueue = 0;
    private int puzzleQueue = 0;

    private int puzzleIndex = 0;

    [SerializeField] private int minPuzzle = 1;
    [SerializeField] private int maxPuzzle = 3;

    public GameObject[] puzzles;

    private bool puzzleJustChanged = false;

    void Start()
    {
        progress = currentProgress;
        puzzles[puzzleIndex].SetActive(true);
        currentPuzzleQueue = Random.Range(minPuzzle, maxPuzzle);
        Debug.Log($"Puzzle index: {puzzleIndex}, Initial queue: {currentPuzzleQueue}");
    }

    void Update()
    {
        // Always check progress
        if (currentProgress != progress)
        {
            ProgressChanged();
        }

        // Only trigger puzzle change once per queue overflow
        if (puzzleQueue > currentPuzzleQueue && !puzzleJustChanged)
        {
            PuzzleChanger();
            puzzleJustChanged = true;
        }

        // Reset flag once queue is matched
        if (puzzleQueue <= currentPuzzleQueue && puzzleJustChanged)
        {
            puzzleJustChanged = false;
        }
    }

    void ProgressChanged()
    {
        puzzleQueue++;
        Debug.Log("ProgressChanged");

        if (currentProgress >= -4 && currentProgress <= 2)
        {
            Debug.Log($"Default state: {currentProgress}");
        }
        else if (currentProgress < -4 && currentProgress > -7)
        {
            Debug.Log($"Enemy is winning state: {currentProgress}");
        }
        else if (currentProgress > 2 && currentProgress < 5)
        {
            Debug.Log($"Player is winning state: {currentProgress}");
        }
        else if (currentProgress >= 5)
        {
            Debug.LogWarning($"Player won: {currentProgress}");
        }
        else if (currentProgress <= -7)
        {
            Debug.LogWarning($"Enemy won: {currentProgress}");
        }

        progress = currentProgress;
    }

    private void PuzzleChanger()
    {
        puzzles[puzzleIndex].SetActive(false);
        puzzleIndex = (puzzleIndex + 1) % puzzles.Length;
        puzzles[puzzleIndex].SetActive(true);
        Debug.Log($"selected index: {puzzleIndex}");

        currentPuzzleQueue = Random.Range(minPuzzle, maxPuzzle);
        Debug.Log($"New puzzle index: {puzzleIndex}, New queue: {currentPuzzleQueue}");
    }
}
