using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform target;
    private Rigidbody2D rb;
    private Vector2 movement;

    private bool isKnocked = false;
    private bool isStunned = false;
    private bool movementEnabled = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
    }

    void Update()
    {
        if (target == null || isKnocked || isStunned || !movementEnabled) return;

        Vector2 direction = (target.position - transform.position).normalized;
        movement = direction;
    }

    void FixedUpdate()
    {
        if (!isKnocked && !isStunned && movementEnabled)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void Knockback(Vector2 force, float duration)
{
    StopAllCoroutines(); // varsa başka knockback iptal
    StartCoroutine(ApplyKnockback(force, duration));
}

IEnumerator ApplyKnockback(Vector2 force, float duration)
{
    isKnocked = true;
    rb.linearVelocity = Vector2.zero; // ✅ Doğru satır bu
    rb.AddForce(force, ForceMode2D.Impulse);
    yield return new WaitForSeconds(duration);
    rb.linearVelocity = Vector2.zero;
    isKnocked = false;
}

    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
    }
}
