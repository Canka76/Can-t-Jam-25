using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        // Ok hareket ediyorsa yönüne doğru döndür
        if (rb.linearVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Target"))
        {
            // 🔎 Hedef özel mi kontrol et
            if (other.gameObject.name.Contains("Special"))
            {
                Debug.Log("ÖZEL HEDEF VURULDU → çift ok aktif!");
                GameManager.Instance.OnTargetHit(); // doubleShot = true
            }
            else
            {
                Debug.Log("Hedef vuruldu!");
            }


            

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
