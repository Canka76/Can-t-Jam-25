using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool doubleShot = false;
    public bool specialTargetSpawned = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnTargetHit()
    {
        doubleShot = true;
    }
}
