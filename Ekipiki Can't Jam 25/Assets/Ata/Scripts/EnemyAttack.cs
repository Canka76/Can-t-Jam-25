using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;
    public int damage = 1;

    private float lastAttackTime;
    private Animator anim;
    private Transform player;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= lastAttackTime)
        {
            anim.SetTrigger("Attack"); // 🔥 Animasyonu tetikle
            player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
