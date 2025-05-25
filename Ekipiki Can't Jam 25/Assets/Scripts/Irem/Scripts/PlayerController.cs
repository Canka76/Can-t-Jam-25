using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public Transform directionArrow;

    [Header("Power Settings")]
    public Slider powerBar;
    public Image fillImage;
    public float maxPower = 20f;
    public float barSpeed = 30f;
    public float shootCooldown = 1f;

    [Header("Animation & Audio")]
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip bowPullSound;
    public AudioClip bowReleaseSound;

    private float currentPower = 0f;
    private bool increasing = true;
    private float lastShootTime = -Mathf.Infinity;

    public float gameDuration = 30f; 
    private float gameTimer = 0f;
    private bool gameOver = false;


    void Update()
    {
        if (gameOver) return;

        gameTimer += Time.deltaTime;
        if (gameTimer >= gameDuration)
        {
            EndGame();
            return;
        }

        RotateDirectionArrow();

        // Mouse basıldığında animasyonu ve çekme sesini başlat
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("isPulling", true);

            if (bowPullSound != null && audioSource != null)
                audioSource.PlayOneShot(bowPullSound);
        }

        // Güç barı kontrolü
        if (Input.GetMouseButton(0))
        {
            if (increasing)
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

        // Bar rengini değiştir
        if (currentPower < maxPower * 0.33f)
            fillImage.color = Color.red;
        else if (currentPower < maxPower * 0.66f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.green;

        // Mouse bırakıldığında ok fırlat ve animasyonu tetikle
        if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("isPulling", false);

            if (Time.time >= lastShootTime + shootCooldown)
            {
                ShootArrow();
                anim.SetTrigger("TriggerShoot");

                if (bowReleaseSound != null && audioSource != null)
                    audioSource.PlayOneShot(bowReleaseSound);

                lastShootTime = Time.time;
                currentPower = 0f;
            }
        }
    }

    void ShootArrow()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 direction = (mousePos - shootPoint.position).normalized;

        // İlk ok
        FireOneArrow(direction);

        // Çift ok varsa
        if (GameManager.Instance != null && GameManager.Instance.doubleShot)
        {
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
        if (Camera.main == null) return;

        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x < 0 || mousePos.y < 0 || mousePos.x > Screen.width || mousePos.y > Screen.height)
            return;

        mousePos.z = 0f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        Vector2 direction = (worldPos - shootPoint.position).normalized;

        if (directionArrow != null)
            directionArrow.right = direction;
    }
    void EndGame()
    {
        gameOver = true;
        Time.timeScale = 0f;
        Debug.Log("Süre doldu! Oyun bitti.");
    }

}
