using UnityEngine;

public class TargetCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎯 Hedef oyuncuya çarptı!");

            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(1);
            }

            Destroy(gameObject); // Hedef kendini yok eder
        }
    }
}
