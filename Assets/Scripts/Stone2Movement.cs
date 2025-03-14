using UnityEngine;

public class Stone2Movement : MonoBehaviour
{
    public float moveSpeed = 3f;       //  Velocidad del movimiento vertical
    public float amplitude = 1.0f;     //  Aumenta la amplitud para mayor movimiento
    private Vector2 startPosition;
    private float timeOffset;

    void Start()
    {
        startPosition = transform.position;
        startPosition.y -= 3.0f; //  Baja MUCHO más la posición inicial
        timeOffset = Random.Range(0f, Mathf.PI * 2); // 🔹 Variación en cada instancia
    }

    void Update()
    {
        //  Movimiento más grande y mucho más abajo
        transform.position = new Vector2(transform.position.x, startPosition.y + Mathf.Sin(Time.time * moveSpeed + timeOffset) * amplitude);
    }
}