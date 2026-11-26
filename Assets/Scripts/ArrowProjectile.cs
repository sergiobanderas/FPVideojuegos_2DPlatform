using System;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] private float arrowSpeed = 10f;
    [SerializeField] private float arrowLifetime = 3f;

    Rigidbody2D rb;    
    
    float direction = 1f; // Default direction is right

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    public void SetDirection(float dir)
    {
        direction = dir;
        // Flip the arrow's sprite based on the direction
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale; 

        rb.linearVelocity = new Vector2(arrowSpeed * direction, 0f);
        Destroy(gameObject, arrowLifetime); 
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Destroy the enemy on impact
            Destroy(gameObject); // Destroy the arrow on impact
            //TODO Add sound effect for arrow hitting enemy
            //TODO Add particle effect for arrow hitting enemy
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.linearVelocity = Vector2.zero; 
        }
    }

    
}
