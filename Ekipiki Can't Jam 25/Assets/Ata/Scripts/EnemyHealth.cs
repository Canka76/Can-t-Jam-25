using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;

    private bool isSlamming = false;
    public bool IsSlamming => isSlamming; // başka yerden erişim için

    private int currentHealth;

    public bool IsThrown = false;
    public bool IsGrabbed { get; private set; } = false;

    private SpriteRenderer sr;
    private Color originalColor;
    private Rigidbody2D rb;
    private EnemyMovement movementScript;
    private Animator anim;

    private int damage => 1;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        rb = GetComponent<Rigidbody2D>();
        movementScript = GetComponent<EnemyMovement>();
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int amount, Vector2 hitDirection, bool useKnockback)
    {
        currentHealth -= amount;
        StartCoroutine(FlashWhite());

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (useKnockback)
        {
            movementScript?.Knockback(hitDirection.normalized * 6f, 0.3f);
        }
        else
        {
            StartCoroutine(FlinchEffect());
        }
    }

    public void Grabbed(Transform parent)
    {
        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;

        movementScript?.SetMovementEnabled(false);
        anim?.SetTrigger("Grabbed");
        IsGrabbed = true;
    }

        public void Slam()
    {
        isSlamming = true;

        transform.SetParent(null);
        rb.isKinematic = false;
        movementScript?.SetMovementEnabled(false);
        IsGrabbed = false;

        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D col in others)
        {
            if (col.CompareTag("Enemy") && col.gameObject != gameObject)
            {
                Vector2 dir = col.transform.position - transform.position;
                col.GetComponent<EnemyHealth>()?.TakeDamage(damage * 2, dir, true);
                col.GetComponent<EnemyMovement>()?.Stun(0.5f);
            }
        }

        TakeDamage(damage * 2, Vector2.zero, false);
        movementScript?.Stun(0.5f);
        StartCoroutine(SlamBounceEffect());
        StartCoroutine(ResumeMovementAfterDelay(1f));
        anim?.SetTrigger("Thrown");

        // 💥 SLAM bitince flag sıfırlanacak
        StartCoroutine(ResetSlamFlag(1f)); // 1 saniye sonra
    }

IEnumerator ResetSlamFlag(float delay)
{
    yield return new WaitForSeconds(delay);
    isSlamming = false;
}


        public void Thrown(Vector2 direction)
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * 20f, ForceMode2D.Impulse);

        IsThrown = true;
        IsGrabbed = false;
        movementScript?.SetMovementEnabled(false);
        anim?.SetTrigger("Thrown");

        StartCoroutine(StopThrowAfterSeconds(0.5f)); // 💥 0.5 saniye sonra dur
    }

        IEnumerator StopThrowAfterSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        IsThrown = false;
        movementScript?.SetMovementEnabled(true); // hareketi geri aç
    }



    IEnumerator ResumeMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        movementScript?.SetMovementEnabled(true);
    }

    IEnumerator FlashWhite()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    IEnumerator FlinchEffect()
    {
        Vector3 originalPos = transform.position;

        for (int i = 0; i < 2; i++)
        {
            transform.position = originalPos + new Vector3(0.05f, 0, 0);
            yield return new WaitForSeconds(0.05f);
            transform.position = originalPos - new Vector3(0.05f, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }

        transform.position = originalPos;
    }

    IEnumerator SlamBounceEffect()
    {
        Vector3 startPos = transform.position;
        float[] bounceHeights = { 0.9f, 0.6f, 0.3f };
        float duration = 0.3f;

        foreach (float height in bounceHeights)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float y = Mathf.Sin((elapsed / duration) * Mathf.PI) * height;
                transform.position = startPos + new Vector3(0, y, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = startPos;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
