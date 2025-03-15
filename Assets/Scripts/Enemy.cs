using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1; // Valor predeterminado

    void Start()
    {
        if (CompareTag("EnemyDarkMouth"))
        {
            health = 3; // Asegura que tenga 3 de vida
            Debug.Log(gameObject.name + " inicia con " + health + " de vida. Tag: " + gameObject.tag);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " recibió " + damage + " de daño. Vida restante: " + health);

        if (health <= 0)
        {
            Debug.Log(gameObject.name + " ha sido destruido.");

            // Aumentamos la puntuación si es un DarkMouth
            GameControls gameControls = FindFirstObjectByType<GameControls>();
            if (gameControls != null && CompareTag("EnemyDarkMouth"))
            {
                gameControls.AddScore(1); // Suma 1 punto por cada DarkMouth
            }

            Destroy(gameObject);
        }
    }

}