using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public Slider powerBar;
    public Image fillImage;
    public Transform directionArrow;

    public float maxPower = 20f;
    public float barSpeed = 30f;

    private float currentPower = 0f;
    private bool increasing = true;

    void Update()
    {
        RotateDirectionArrow();
        // Otomatik dolup boşalan gü barı
        if (Input.GetMouseButton(0))
        {if (increasing)
            {
                currentPower += Time.deltaTime * barSpeed;
                if (currentPower >= maxPower)
                {
                    currentPower = maxPower;
                    increasing = false;
                }
            }
            else
            {
                currentPower -= Time.deltaTime * barSpeed;
                if (currentPower <= 0f)
                {
                    currentPower = 0f;
                    increasing = true;
                }
            }
        }

        powerBar.value = currentPower;

        // Renk değiştirme (doluluğa göre)
        if (currentPower < maxPower * 0.33f)
            fillImage.color = Color.red;
        else if (currentPower < maxPower * 0.66f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.green;

        // Tıklama ile fırlatma
        if (Input.GetMouseButtonUp(0))
        {
            ShootArrow();
            currentPower = 0f;
        }
    }

    void ShootArrow()
    {
        Debug.Log("Çift atış durumu: " + GameManager.Instance.doubleShot);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 direction = (mousePos - shootPoint.position).normalized;

        FireOneArrow(direction);

        // İkinci ok 
        if (GameManager.Instance.doubleShot)
            FireOneArrow(direction);

        if (GameManager.Instance != null && GameManager.Instance.doubleShot)
        {
            // Hafif açılı ikinci ok
            Vector2 secondDirection = Quaternion.Euler(0, 0, 10) * direction;
            FireOneArrow(secondDirection);
        }
    }

    void FireOneArrow(Vector2 direction)
    {
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        arrow.transform.right = direction;

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * currentPower, ForceMode2D.Impulse);
    }


    void RotateDirectionArrow()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - shootPoint.position).normalized;

        if (directionArrow != null)
            directionArrow.right = direction;
    }

}
