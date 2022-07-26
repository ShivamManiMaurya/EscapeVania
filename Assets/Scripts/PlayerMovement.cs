using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;

    Vector2 moveInputs;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        
    }

    void Update()
    {
        Run();
        FlipSprite();

        
    }

    void OnMove(InputValue value)
    {
        moveInputs = value.Get<Vector2>();
    }
    
    void OnJump(InputValue value)
    {
        //***************************
        // Other Method

        //if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        //if (value.isPressed)
        //{
        //    myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        //}
        //*******************************


        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void ClimbLadder(InputValue value)
    {
        value.isPressed.Equals("W");

        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidbody.position += new Vector2(0f, 1f);
        }
    }


    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInputs.x * runSpeed, myRigidbody.velocity.y); 
        myRigidbody.velocity = playerVelocity;

        bool playerIsRunningTransition = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        myAnimator.SetBool("isRunning", playerIsRunningTransition);
    }


    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }






}