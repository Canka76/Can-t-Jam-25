using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Target"))
        {
            Debug.Log("Hedef vuruldu!");
            ScoreManager.Instance.AddScore(1);// Skor artýr
            //Destroy(other.gameObject); // hedefi yok et
            Destroy(gameObject); // oku da yok et
        }
    }
}
