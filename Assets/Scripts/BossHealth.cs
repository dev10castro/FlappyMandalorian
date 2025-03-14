using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int health = 30; // Puedes ajustar la vida del boss según necesites

    private GameControls gameControls;

    void Start()
    {
        gameControls = FindFirstObjectByType<GameControls>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Notificamos que el Boss ha sido derrotado
        if (gameControls != null)
        {
            gameControls.YouWin();
        }

        Destroy(gameObject); // 🔹 Eliminamos el Boss
    }
}