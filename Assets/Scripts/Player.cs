using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public float jumpForce = 50f;
    public float moveSpeed = 7f;
    public GameObject bullet;
    public Transform firePoint;
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip jumpSound;
    public AudioClip deathSound;

    private GameControls gameControls;
    public bool isFlor = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameControls = FindFirstObjectByType<GameControls>();

        if (gameControls == null)
        {
            Debug.LogError("No se encontró GameControls en la escena.");
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isFlor)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isFlor = false;
            
            if (jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }

        if (rb.linearVelocity.y != 0)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            isFlor = true;
        }

        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if (move != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Shoot();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard") || collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("EnemyDarkMouth"))
        {
            if (deathSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(deathSound);
            }

            gameControls.GameOver(); // Muestra Game Over
            Invoke("DestroyPlayer", 1.0f); // Retrasa la destrucción del jugador 1 segundo
            audioSource.Stop();
        }
    }

    void DestroyPlayer()
    {
        Destroy(gameObject);
    }

    void Shoot()
    {
        Vector3 firePosition = firePoint.position + new Vector3(0.5f, 0, 0);
        GameObject newBullet = Instantiate(bullet, firePosition, firePoint.rotation);
        Debug.Log("Bala generada en: " + firePosition);

        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
