using UnityEngine;

public class ThrownHitbox : MonoBehaviour
{
    private EnemyHealth parent;

    void Start()
    {
        parent = GetComponentInParent<EnemyHealth>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (parent == null || !parent.IsThrown) return;
        if (other.CompareTag("Enemy") && other.gameObject != transform.parent.gameObject)
        {
            Debug.Log("🔥 YOL ÜSTÜ HASAR VERİLDİ");
            other.GetComponent<EnemyHealth>()?.TakeDamage(2, Vector2.zero, false);
            // ❌ Durdurma yok, yoluna devam etsin
        }
    }
}
