using UnityEngine;

public class GameManagerAta : MonoBehaviour
{
    public static GameManagerAta Instance;

    public int killCount = 0;
    public int winKillCount = 9;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void EnemyKilled()
    {
        killCount++;
        Debug.Log("☠️ Öldürülen düşman: " + killCount);

        if (killCount >= winKillCount)
        {
            GameOver(true);
        }
    }

    public void GameOver(bool win)
    {
        if (win)
        {
            Debug.Log("🎉 Kazandın kanka!");
            // Buraya victory ekranı gelecek
        }
        else
        {
            Debug.Log("💀 Kaybettin kanka!");
        }

        Time.timeScale = 0f; // Oyun dursun
    }
}
