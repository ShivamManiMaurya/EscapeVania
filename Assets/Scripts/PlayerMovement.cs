using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] bool onLadder;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;

    float myGavityScaleAtStart;
    bool isAlive = true;
    

    Vector2 moveInputs;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    
    

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myGavityScaleAtStart = myRigidbody.gravityScale;

        
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
        
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(arrow, bow.position, transform.rotation);

        myAnimator.SetTrigger("isShooting");
    }


    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInputs = value.Get<Vector2>();
    }

    void Run()
    {
        if (!isAlive) { return; }
        Vector2 playerVelocity = new Vector2(moveInputs.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerIsRunningTransition = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        myAnimator.SetBool("isRunning", playerIsRunningTransition);
    }

    void OnJump(InputValue value)
    {
        //***************************
        // Other Method

        //if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        //if (value.isPressed)
        //{
        //    myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        //}
        //*******************************

        if (!isAlive) { return; }
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !onLadder)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }

    }

    void ClimbLadder()
    { 
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = myGavityScaleAtStart;
            return;
        }

        Vector2 playerClimbingVelocity = new Vector2(myRigidbody.velocity.x, moveInputs.y * climbSpeed);
        myRigidbody.velocity = playerClimbingVelocity;

        bool playerHasClimbingVelocity = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasClimbingVelocity);

        myRigidbody.gravityScale = 0;

    }



    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }


    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazard")))
        {
            myRigidbody.velocity = new Vector2(0f, jumpSpeed);
            myBodyCollider.isTrigger = true;
            myAnimator.SetTrigger("Dying");
            isAlive = false;

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    

    

}
