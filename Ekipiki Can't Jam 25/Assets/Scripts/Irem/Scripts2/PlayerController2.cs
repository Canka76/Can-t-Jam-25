using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 20f;
    private Rigidbody2D rb;
    private Vector3 originalScale;
    private BoxCollider2D col;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Z�plama
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        // E�ilme
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
            col.offset = new Vector2(0, -0.25f);
            col.size = new Vector2(1, 0.5f);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            transform.localScale = originalScale;
            col.offset = new Vector2(0, 0);
            col.size = new Vector2(1, 1);
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
}
