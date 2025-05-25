using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public float iFrameDuration = 1f;
    public SpriteRenderer sr; // Sprite için renk efekti
    public Color flashColor = Color.red;
    public int flashCount = 3;

    private int currentHealth;
    private bool isInvincible = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        Debug.Log("💢 Hasar aldın! Can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ActivateIFrames());
        }
    }

    IEnumerator ActivateIFrames()
    {
        isInvincible = true;

        for (int i = 0; i < flashCount; i++)
        {
            sr.color = flashColor;
            yield return new WaitForSeconds(iFrameDuration / (flashCount * 2));
            sr.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (flashCount * 2));
        }

        isInvincible = false;
    }

        public bool Heal(int amount)
    {
        if (currentHealth >= maxHealth)
            return false;

        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log("💚 Can topladın! Yeni Can: " + currentHealth);
        return true;
    }


    void Die()
    {
        Debug.Log("☠️ Öldün.");
        // Ölüm animasyonu vs.
    }
}
