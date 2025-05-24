using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private int progress = 0;
    public int currentProgress = 0;

    private int puzzleQueue = 0;
    private int puzzleIndex = 0;

    [SerializeField] private int minPuzzle = 1;
    [SerializeField] private int maxPuzzle = 3;

    public GameObject[] puzzles;

    private void Start()
    {
        progress = currentProgress;
        puzzles[puzzleIndex].SetActive(true);
        puzzleQueue = 0;
        Debug.Log($"Puzzle index: {puzzleIndex}, Initial queue: {puzzleQueue}");
    }

    private void Update()
    {
        if (currentProgress != progress)
        {
            ProgressChanged();
        }

        if (puzzleQueue >= Random.Range(minPuzzle, maxPuzzle))
        {
            PuzzleChanger();
            puzzleQueue = 0;
        }

        
    }

    private void ProgressChanged()
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

        Debug.Log($"New puzzle index: {puzzleIndex}");
    }
}