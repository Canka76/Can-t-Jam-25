using UnityEngine;

public class HealthBox : MonoBehaviour
{
    public int healthAmount = 1; // Verilecek can miktarı
    public GameObject destroyEffect; // Partikül vs.
    public AudioClip healSound; // Ses efekti
    private bool used = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                bool healed = player.Heal(healthAmount);
                if (healed)
                {
                    used = true;

                    if (destroyEffect != null)
                        Instantiate(destroyEffect, transform.position, Quaternion.identity);

                    if (healSound != null)
                        AudioSource.PlayClipAtPoint(healSound, transform.position);

                    Destroy(gameObject); // Kutu yok olsun
                }
            }
        }
    }
}
