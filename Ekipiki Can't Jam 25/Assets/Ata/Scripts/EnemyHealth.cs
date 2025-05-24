using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public bool IsThrown() => isThrown;
    public int maxHealth = 3;
    private int currentHealth;

    private bool isThrown = false;

    private SpriteRenderer sr;
    private Color originalColor;
    private Rigidbody2D rb;
    private EnemyMovement movementScript;

    private int damage => 1;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        rb = GetComponent<Rigidbody2D>();
        movementScript = GetComponent<EnemyMovement>();
    }

    public void TakeDamage(int amount, Vector2 hitDirection, bool useKnockback)
    {
        currentHealth -= amount;
        StartCoroutine(FlashWhite());

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (useKnockback)
            movementScript?.Knockback(hitDirection.normalized * 6f, 0.3f);
        else
            StartCoroutine(FlinchEffect());
    }

    public void Grabbed(Transform parent)
{
    rb.isKinematic = true;
    rb.linearVelocity = Vector2.zero;
    transform.SetParent(parent);
    transform.localPosition = Vector3.zero;

    movementScript?.SetMovementEnabled(false);
    GetComponent<Animator>()?.SetTrigger("Grabbed"); // 🔥 ANİMASYON
}

    public void Slam()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        movementScript?.SetMovementEnabled(false);

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

        GetComponent<Animator>()?.SetTrigger("Grabbed");
    }

    public void Thrown(Vector2 direction)
{
    transform.SetParent(null);
    rb.isKinematic = false;
    rb.linearVelocity = Vector2.zero;
    rb.AddForce(direction.normalized * 20f, ForceMode2D.Impulse);

    isThrown = true;
    movementScript?.SetMovementEnabled(false);

    GetComponent<Animator>()?.SetTrigger("Thrown"); // 🔥 ANİMASYON
}



    IEnumerator DestroyAfterFlight()
    {
        yield return new WaitForSeconds(0.5f);
        TakeDamage(damage * 2, Vector2.zero, false);
        movementScript?.SetMovementEnabled(true);
        isThrown = false;
    }

    IEnumerator StopThrowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isThrown) // hâlâ fırlatılmışsa
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            isThrown = false;
            movementScript?.SetMovementEnabled(true);
        }
    }

void OnCollisionEnter2D(Collision2D collision)
{
    if (isThrown && collision.gameObject.CompareTag("Enemy") && collision.gameObject != gameObject)
    {
        Debug.Log("💥 FİZİKSEL ÇARPTIM!");

        // Yön vektörü: çarpılan düşmanın fırlatana göre yönü
        Vector2 dir = collision.transform.position - transform.position;
        dir.Normalize();

        // Çarpılan düşmana hasar + knockback
        var otherHealth = collision.gameObject.GetComponent<EnemyHealth>();
        var otherMove = collision.gameObject.GetComponent<EnemyMovement>();

        otherHealth?.TakeDamage(damage * 2, dir, true);
        otherMove?.Knockback(dir * 5f, 0.2f);

        // Fırlayan düşman da biraz sekecek gibi hafif geri tepki
        Vector2 recoilDir = -dir;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(recoilDir * 3f, ForceMode2D.Impulse); // çarpan geri savrulur

        // Fırlatma bitiyor
        isThrown = false;
        movementScript?.SetMovementEnabled(true);
    }
}


    

    IEnumerator ResumeMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        movementScript?.SetMovementEnabled(true);
    }

    IEnumerator FlashWhite()
    {
        sr.color = Color.white;
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
