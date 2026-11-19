using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float shootCooldown = 0.5f; // Cooldown time in seconds

    Rigidbody2D rb;
    Animator animator;
    BoxCollider2D myFeetCollider;
    Transform arrowSpawnPoint;


    Vector2 moveInput;
    float gravityScaleAtStart;
    float lastShootTime = -999f; // Track the last time the player shot an arrow

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        animator = transform.GetChild(0).GetChild(selectedCharacterIndex).GetComponent<Animator>();
        gravityScaleAtStart = rb.gravityScale;
        arrowSpawnPoint = transform.GetChild(1).GetComponent<Transform>();
    }

    void Update()
    {
        Run();
        IsJumping();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && value.isPressed)
        {
            rb.linearVelocity += new Vector2(0f, jumpForce);            
        }        
    }

    
    void OnAttack(InputValue value)
    {   
        if (!value.isPressed) return; // Only trigger on button press, not release
        if (animator.GetBool("isClimbing")) return; // Prevent shooting while climbing
        
        if (Time.time < lastShootTime +  shootCooldown) return; // Prevent shooting too quickly
        lastShootTime = Time.time;

        animator.SetTrigger("Shoot");        
    }

    public void ShootArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        arrow.GetComponent<ArrowProjectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private void IsJumping()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")) )
        {
            animator.SetBool("isJumping", true);            
        }
        else
        {
            animator.SetBool("isJumping", false);            
        }
        
    }


    private void Run()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
       
        animator.SetBool("isRunning", moveInput.x != 0f);

        if (moveInput.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (moveInput.x > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }   

    private void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            rb.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;    
        }

        rb.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(rb.linearVelocity.x, moveInput.y * moveSpeed);
        rb.linearVelocity = climbVelocity;

        animator.SetBool("isClimbing", moveInput.y != 0f);
    }


}
