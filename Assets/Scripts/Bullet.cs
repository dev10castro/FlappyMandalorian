using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed; // Aplica velocidad a la bala
        }
        else
        {
            Debug.LogError("No se encontró Rigidbody2D en la bala.");
        }

        Destroy(gameObject, lifetime); // Destruir la bala tras el tiempo definido
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bala impactó con: " + collision.gameObject.name);

        if (collision.CompareTag("Obstacle"))
        {
            if (collision.gameObject.name.Contains("Boss"))
            {
                BossHealth bossHealth = collision.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(1);
                }
            }
            else
            {
                Destroy(collision.gameObject); // Destruye enemigos normales
            }
            Destroy(gameObject); // La bala desaparece después de impactar
        }
        else if (collision.CompareTag("EnemyDarkMouth")) // Ahora detecta a DarkMouth
        {
            Debug.Log("DarkMouth impactado por una bala.");
            Enemy enemyScript = collision.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(1); // Resta 1 de vida
            }
            Destroy(gameObject); // La bala desaparece
        }
        else if (collision.CompareTag("Hazard"))
        {
            Debug.Log("Impactó un Hazard, pero este no será destruido: " + collision.gameObject.name);
            Destroy(gameObject); // La bala desaparece, pero el Hazard permanece
        }
        else if (!collision.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }
}
