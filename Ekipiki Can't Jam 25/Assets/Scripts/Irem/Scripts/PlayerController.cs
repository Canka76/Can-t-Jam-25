using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public Slider powerBar;
    public Image fillImage;

    //public float arrowForce = 10f;
    public float maxPower = 20f;
    private float currentPower = 0f;
    //private bool isCharging = false;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            currentPower += Time.deltaTime * 10f;
            currentPower = Mathf.Clamp(currentPower, 0f, maxPower);
            powerBar.value = currentPower;

            // Renk deðiþtirme:
            if (currentPower < maxPower * 0.33f)
                fillImage.color = Color.red;
            else if (currentPower < maxPower * 0.66f)
                fillImage.color = Color.yellow;
            else
                fillImage.color = Color.green;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ShootArrow();
            currentPower = 0f;
            powerBar.value = 0f;
            fillImage.color = Color.red; // sýfýrlanýnca baþlangýç rengi
        }
    }


    void ShootArrow()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 direction = (mousePos - shootPoint.position).normalized;

        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        arrow.transform.right = direction;

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * currentPower, ForceMode2D.Impulse);
    }
}
