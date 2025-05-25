using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 20f;
    private Rigidbody2D rb;
    private Vector3 originalScale;
    private BoxCollider2D col;
    private bool isGrounded = true;

    float slideTimer = 0f;
    float slideDuration = 1f;
    bool isSliding = false;

    public float gameDuration = 30f; // Toplam oyun süresi
    private float gameTimer = 0f;
    private bool gameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Zıplama
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // velocity kullan
            isGrounded = false;
        }

        // Eğilmeyi başlat
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isSliding)
        {
            isSliding = true;
            slideTimer = 0f;

            transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
            col.offset = new Vector2(0, -0.25f);
            col.size = new Vector2(1, 0.5f);
        }

        // Eğilme süresi takip
        if (isSliding)
        {
            slideTimer += Time.deltaTime;
            if (slideTimer >= slideDuration)
            {
                isSliding = false;
                transform.localScale = originalScale;
                col.offset = new Vector2(0, 0);
                col.size = new Vector2(1, 1);
            }
        }

        // ⏱ Oyun süresi kontrolü
        if (!gameOver)
        {
            gameTimer += Time.deltaTime;

            if (gameTimer >= gameDuration)
            {
                EndGame();
            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.collider.CompareTag("Obstacle"))
        {
            Debug.Log("Engelle çarpışıldı!");

            // Tüm MoveLeft scriptlerini durdur
            MoveLeft[] allMoving = FindObjectsOfType<MoveLeft>();
            foreach (MoveLeft move in allMoving)
            {
                move.enabled = false; // hareketi durdurur
            }

            // ObstacleSpawnerObj objesini tamamen devre dışı bırak
            GameObject spawnerObj = GameObject.Find("ObstacleSpawnerObj");
            if (spawnerObj != null)
                spawnerObj.SetActive(false);

            // Oyuncunun zıplamasını da durdurmak istersen:
            this.enabled = false;
        }
    }
    void EndGame()
    {
        gameOver = true;
        Time.timeScale = 0f; // 🛑 Oyun durur
        Debug.Log("⏰ 20 saniye doldu! Oyun bitti.");

        // İsteğe bağlı: UI göster, restart butonu aç, skor yazdır vs.
    }

}
