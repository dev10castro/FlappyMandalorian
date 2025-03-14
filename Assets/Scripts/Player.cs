using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public float jumpForce = 50f;
    public float moveSpeed = 7f;
    public GameObject bullet; // Prefab del proyectil
    public Transform firePoint; // Punto desde donde se disparará el proyectil

    
    private bool isFloor = true; 

    void Start()
    {
        
        
        
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Space) && isFloor)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isFloor = false;
        }

        if (rb.linearVelocity.y != 0)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            isFloor = true;
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

        // Disparar con la tecla "N"
        if (Input.GetKeyDown(KeyCode.N))
        {
            Shoot();
        }
        
       
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Hazard"))
        {
            Destroy(gameObject);
            
        }
        
        
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}