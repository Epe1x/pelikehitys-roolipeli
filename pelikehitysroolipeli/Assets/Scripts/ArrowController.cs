using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float speed = 10;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Collider2D arrowCol = GetComponent<Collider2D>();
        Collider2D playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(arrowCol, playerCol);
    }

    public void Launch(Vector2 direction)
    {
        if (rb == null)
        {
            return;
        }

        rb.linearVelocity = direction * speed;
    }

    public ArrowType arrowType;
    private int GetDamage()
    {
        switch (arrowType)
        {
            case ArrowType.Aloittelijanuoli:
                return 1;
            case ArrowType.Perusnuoli:
                return 10;
            case ArrowType.Eliittinuoli:
                return 30;
            default:
                return 1;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            Debug.Log("Arrow hit: " + other.gameObject.name);
            damageable.TakeDamage(GetDamage());
            Destroy(gameObject); 
        }

        if (!other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Arrow hit: " + other.gameObject.name);
            Destroy(gameObject);
        }
    }
}
