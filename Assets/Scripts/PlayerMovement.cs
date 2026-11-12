using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    

    Rigidbody2D rb;
    Animator animator;
    BoxCollider2D myFeetCollider;


    Vector2 moveInput;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        animator = transform.GetChild(0).GetChild(selectedCharacterIndex).GetComponent<Animator>();
    }

    void Update()
    {
        Run();
        IsJumping();
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

    private void IsJumping()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
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


}
