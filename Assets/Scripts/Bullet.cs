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
            rb.linearVelocity = transform.right * speed; // Mueve la bala hacia adelante
        }
        else
        {
            Debug.LogError("No se encontró Rigidbody2D en la bala.");
        }

        Destroy(gameObject, lifetime); // Destruye la bala después de un tiempo
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bala colisionó con: " + collision.gameObject.name);

        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("Impactó un obstáculo: " + collision.gameObject.name);

            // Verificar si el obstáculo tiene un script Enemy (evita errores si no tiene)
            Enemy enemyScript = collision.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(1); // Aplica daño solo si tiene script
            }
            else
            {
                Destroy(collision.gameObject); // Si no tiene script, se destruye directamente
            }

            Destroy(gameObject); // La bala se destruye siempre al impactar
        }
    }

}