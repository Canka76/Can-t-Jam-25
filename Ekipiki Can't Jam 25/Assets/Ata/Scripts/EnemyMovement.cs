using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 1.2f;

    private Transform target;
    private Rigidbody2D rb;
    private Vector2 movement;

    private bool isKnocked = false;
    private bool isStunned = false;
    private bool movementEnabled = true;

    private Animator anim;
    private EnemyHealth health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
    }

    void Update()
    {
        if (target == null || isKnocked || isStunned || !movementEnabled || (health != null && health.IsThrown))
        {
            anim?.SetBool("IsWalking", false);
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange)
        {
            movement = Vector2.zero;
            anim?.SetBool("IsWalking", false);
            return; // ➤ Saldırı menziline girince dur
        }

        Vector2 direction = (target.position - transform.position).normalized;
        movement = direction;

        // Animasyon
        anim?.SetBool("IsWalking", true);

        // Yüzünü çevir
        if (direction.x > 0.05f)
            transform.localScale = new Vector3(2f, 2f, 1f); // sağa
        else if (direction.x < -0.05f)
            transform.localScale = new Vector3(-2f, 2f, 1f); // sola
    }

    void FixedUpdate()
    {
        if (!isKnocked && !isStunned && movementEnabled && (health == null || !health.IsThrown))
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void Knockback(Vector2 force, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ApplyKnockback(force, duration));
    }

    IEnumerator ApplyKnockback(Vector2 force, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = Vector2.zero;
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
