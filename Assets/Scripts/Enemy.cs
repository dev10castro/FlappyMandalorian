using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3; // Vida predeterminada de los enemigos
    public bool isBoss = false; // Si es un Boss, tendrá más vida

    void Start()
    {
        if (isBoss)
        {
            health = 30; // 🔹 Asegura que el Boss tenga 20 de vida
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " recibió " + damage + " de daño. Vida restante: " + health);

        if (health <= 0)
        {
            Debug.Log(gameObject.name + " ha sido destruido.");
            Destroy(gameObject);
        }
    }
}