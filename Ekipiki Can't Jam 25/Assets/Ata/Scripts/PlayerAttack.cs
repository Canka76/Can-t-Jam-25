using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int damage = 1;
    public float comboResetTime = 1.5f;

    private Animator anim;
    private bool canAttack = true;

    private int comboCounter = 0;
    private float lastAttackTime;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboCounter = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canAttack)
        {
            comboCounter++;
            PlayComboAnimation(comboCounter);
            Attack();
            canAttack = false;
            StartCoroutine(AttackRate());
            lastAttackTime = Time.time;

            if (comboCounter >= 3)
            {
                comboCounter = 0;
            }
        }

    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Vector2 hitDirection = enemy.transform.position - transform.position;
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (comboCounter >= 3)
            {
                // Son vuruş: Knockback'li uçma
                enemyHealth.TakeDamage(damage, hitDirection, true);
                comboCounter = 0;
            }
            else
            {
                // Hafif sarsılma efekti
                enemyHealth.TakeDamage(damage, hitDirection, false);
            }
        }
    }

    IEnumerator AttackRate()
    {
        yield return new WaitForSeconds(0.1f);
        canAttack = true;
    }

        void PlayComboAnimation(int combo)
    {
        switch (combo)
        {
            case 1:
                anim.SetTrigger("Attack1");
                break;
            case 2:
                anim.SetTrigger("Attack2");
                break;
            case 3:
                anim.SetTrigger("Attack3");
                break;
            default:
                anim.SetTrigger("Attack1");
                break;
        }
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
