using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;
    public int damage = 1;
    public float windupTime = 1f; // ⏱ Vurmadan önceki bekleme süresi

    private float lastAttackTime;
    private Animator anim;
    private Transform player;
    private bool isAttacking = false;

    private EnemyHealth health; // script başında

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GetComponent<EnemyHealth>(); // bağla
    }


    void Update()
    {
        if (player == null || isAttacking) return;

        if (health != null && (health.IsGrabbed || health.IsThrown || health.IsSlamming)) return; // ❌ GRABBEDKEN SALDIRMA

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= lastAttackTime)
        {
            anim.SetTrigger("Attack");
            StartCoroutine(DelayedAttack());
            lastAttackTime = Time.time;
        }
}


    IEnumerator DelayedAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(windupTime);

        if (player != null && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }

        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
