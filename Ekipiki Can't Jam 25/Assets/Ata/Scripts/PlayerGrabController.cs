using UnityEngine;

public class PlayerGrabController : MonoBehaviour
{
    public Transform grabPoint;
    private EnemyHealth grabbedEnemy;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabbedEnemy == null)
            {
                TryGrab();
            }
            else
            {
                SlamDown();
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && grabbedEnemy != null)
        {
            ThrowForward();
        }
    }

    void TryGrab()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(grabPoint.position, 1f);

        foreach (Collider2D col in enemies)
        {
            if (col.CompareTag("Enemy"))
            {
                grabbedEnemy = col.GetComponent<EnemyHealth>();
                if (grabbedEnemy != null)
                {
                    grabbedEnemy.Grabbed(grabPoint);
                    break;
                }
            }
        }
    }

    void SlamDown()
    {
        grabbedEnemy.Slam();
        grabbedEnemy = null;
    }

    void ThrowForward()
{
    if (grabbedEnemy != null)
    {
        float directionX = Mathf.Sign(transform.localScale.x); // -1 sola, +1 sağa
        Vector2 throwDir = new Vector2(directionX, 0f);
        grabbedEnemy.Thrown(throwDir);
        grabbedEnemy = null;
    }
}

}
