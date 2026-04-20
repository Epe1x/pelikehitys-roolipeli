using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour, IDamageable
{
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void TakeDamage(int amount)
    {
        float disappearTime = amount * 0.1f;
        StartCoroutine(Disappear(disappearTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(20);
        }
    }

    private IEnumerator Disappear(float disappearTime)
    {
        spriteRenderer.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(disappearTime);

        spriteRenderer.enabled = true;
        col.enabled = true;
    }
}