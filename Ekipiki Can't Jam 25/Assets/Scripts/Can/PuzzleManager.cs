using System.Security.Cryptography;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private int progress = 0;
    public int currentProgress = 0;

    private int puzzleQueue = 0;
    private int puzzleIndex = 0;

    [SerializeField] private float hitstopTrigger = 0.1f;

    [SerializeField] private int minPuzzle = 1;
    [SerializeField] private int maxPuzzle = 3;

    public GameObject[] puzzles;
    private Hitstop Hitstop;

    public GameObject PlayGameObject;
    public GameObject EnemGameObject;

    private Animator playAnimator;
    private Animator enemAnimator;

    private void Start()
    {
        progress = currentProgress;
        puzzles[puzzleIndex].SetActive(true);
        puzzleQueue = 0;

        Debug.Log($"Puzzle index: {puzzleIndex}, Initial queue: {puzzleQueue}");

        playAnimator = PlayGameObject.GetComponent<Animator>();
        enemAnimator = EnemGameObject.GetComponent<Animator>();
        
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
        //Hitstop.Instance.Trigger(hitstopTrigger);
        Debug.LogError("Hit stop triggered");

        puzzleQueue++;
        Debug.Log("ProgressChanged");

        if (currentProgress >= -4 && currentProgress <= 2)
        {
            Debug.Log($"Default state: {currentProgress}");
            playAnimator.SetTrigger("DefaultState");
            enemAnimator.SetTrigger("DefaultState");
            
        }
        else if (currentProgress < -4 && currentProgress > -7)
        {
            Debug.Log($"Enemy is winning state: {currentProgress}");
            playAnimator.SetTrigger("EnemyWinning");
            enemAnimator.SetTrigger("PlayerWinning");
        }
        else if (currentProgress > 2 && currentProgress < 5)
        {
            Debug.Log($"Player is winning state: {currentProgress}");
            playAnimator.SetTrigger("PlayerWinning");
            enemAnimator.SetTrigger("EnemyWinning");

        }
        else if (currentProgress >= 5)
        {
            Debug.LogWarning($"Player won: {currentProgress}");
            playAnimator.SetTrigger("PlayerWon");
            enemAnimator.SetTrigger("EnemyWon");

        }
        else if (currentProgress <= -7)
        {
            Debug.LogWarning($"Enemy won: {currentProgress}");
            playAnimator.SetTrigger("EnemyWon");
            enemAnimator.SetTrigger("PlayerWon");
            
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