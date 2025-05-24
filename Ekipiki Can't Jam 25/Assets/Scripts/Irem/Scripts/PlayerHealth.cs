using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public Text healthText; // UI'da canı göstermek için

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UpdateHealthUI();

        Debug.Log("Şu anki can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Oyuncu öldü!");
        // Oyuncuyu yok etmek istemiyorsan buraya Game Over UI ekleyebilirsin
        // gameObject.SetActive(false); // Pasif yapabilirsin
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "Can: " + currentHealth;
    }
}
