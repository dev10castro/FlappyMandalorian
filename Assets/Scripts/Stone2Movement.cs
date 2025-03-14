using UnityEngine;

public class Stone2Movement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float amplitude = 1.5f; // Altura máxima del movimiento
    private Vector2 startPosition;
    private float timeOffset;

    void Start()
    {
        startPosition = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2); // Para variar el movimiento de cada instancia
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x, startPosition.y + Mathf.Sin(Time.time * moveSpeed + timeOffset) * amplitude);
    }
}