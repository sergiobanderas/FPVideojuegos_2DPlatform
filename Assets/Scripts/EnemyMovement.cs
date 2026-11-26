using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float wallCheckDistance = 0.5f; // Distance to check for walls
    [SerializeField] float groundCheckDistance = 1f; // Distance to check for ground
    [SerializeField] float groundCheckOffset = 0.1f; // Offset for ground check
    [SerializeField] bool enemyDetection = false;

    Rigidbody2D rb;

    enum facing
    {
        Left,
        Right
    }
    
    [SerializeField] facing startDirection = facing.Right;

    int direction = 1; // 1 for right, -1 for left

    void Start()
    {
        if (startDirection == facing.Left)
        {
            TurnAround();
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (IsTouchingWall() || IsAtEdge() )
        {
            TurnAround();
        }

       rb.linearVelocity = new Vector2(moveSpeed * direction, 0f);
    }


    private bool IsAtEdge()
    {
        Vector2 origin = rb.position + new Vector2(groundCheckOffset * direction, 0f);

        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, Color.blue);

        // Check for ground using a raycast
        return !Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
    }

    /// <summary>
    /// Checks if the enemy is touching a wall in the direction it is moving
    /// </summary>
    /// <returns></returns>
    private bool IsTouchingWall()
    {
        Vector2 origin = rb.position;
        Vector2 rayDirection = Vector2.right * direction;

        Debug.DrawRay(origin, rayDirection * wallCheckDistance, Color.red);

        // Check for walls using a raycast
        return Physics2D.Raycast(origin, rayDirection, wallCheckDistance, LayerMask.GetMask("Ground"));


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && enemyDetection)
        {
            TurnAround();
        }

        if (other.CompareTag("Player"))
        {
            if (IsStompedByPlayer(other))
            {            
                other.GetComponent<PlayerMovement>().Bounce(); 
                Destroy(gameObject); // Destroy the enemy on stomp

            }
            else
            {
                other.GetComponent<PlayerHealth>().TakeDamage();
            }
            
        }
    }

    private bool IsStompedByPlayer(Collider2D player)
    {
        float verticalSpeed = player.attachedRigidbody.linearVelocity.y;
        return verticalSpeed < 0f; // Check if the player is falling
    }




    /// <summary>
    /// Flips the enemy's direction 
    /// </summary>
    private void TurnAround()
    {
        direction = -direction;
        transform.localScale = new Vector2(direction, 1f); // Flip the enemy's sprite
    }

    
}
