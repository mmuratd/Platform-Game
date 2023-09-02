using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f; 
    [SerializeField] float jumpCount = 2f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gunTransform;

    bool isAlive = true;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2d;
    BoxCollider2D myFeetCollider2D;
    CharacterController cc;
    float currentJumpCount = 2f;
    bool canJump = true;

    float defaultGravity;
    [SerializeField] float delayTimeOfDeath = 3f;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myBodyCollider2d = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        defaultGravity = myRigidbody.gravityScale;
        
    }

    void Update()
    {
        if (!isAlive)
        {
            if (IsTouchingGround())
            {
                myAnimator.SetTrigger("Died");
            }
        }
        ClimbLadder();
        JumpReset();
        Run();
        Fall();
        FlipSprite();
        Die();
    }

    void Run()
    {
        if (isAlive)
        {
            
            Vector2 playerVelocity = new Vector2(moveInput.x * speed, myRigidbody.velocity.y);

            myRigidbody.velocity = playerVelocity;
            bool hasHorizontalSpeed = myRigidbody.velocity.x != 0;
            if (hasHorizontalSpeed)
            {
                myAnimator.SetBool("isRunning", true);
            }
            else
            {
                myAnimator.SetBool("isRunning", false);
            }
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = myRigidbody.velocity.x != 0;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void OnJump(InputValue value)
    {
        if (currentJumpCount > 0)
        {
            if (value.isPressed)
            {
                currentJumpCount--;
                if (myRigidbody.velocity.y < 0)
                {
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0);
                }
                myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            }
        }
    }

    void OnFire(InputValue value){
        if (isAlive)
        {
            Instantiate(bullet, gunTransform.position, transform.rotation);
            
        }
    }
    void Fall()
    {
        Vector2 playerFallVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * jumpSpeed);
        if (moveInput.y < 0)
        {
            
            if (myRigidbody.velocity.y > -20f)
            {
                myRigidbody.velocity = playerFallVelocity;
            }
        }
    }

    void Die()
    {
        bool dieLayer = myBodyCollider2d.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard"));
        if (dieLayer)
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity += new Vector2(-moveInput.x * 10f, 20f);
            StartCoroutine(DeathDelay());
            
        }
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSecondsRealtime(delayTimeOfDeath);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();

    }
    bool IsTouchingGround()
    {
        bool touchingLayer = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        return touchingLayer;
    }

    void JumpReset()
    {
        if (IsTouchingGround())
        {
            canJump = true;
            currentJumpCount = jumpCount;
        }
    }

    void ClimbLadder()
    {
        canJump = false;
        if (myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            currentJumpCount = jumpCount;
            myRigidbody.gravityScale = 0;
            bool hasVerticalSpeed = myRigidbody.velocity.y != 0;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
            if (hasVerticalSpeed)
            {
                myAnimator.enabled = true;
                myAnimator.SetBool("isClimbing", true);
            }
            else
            {
                if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
                {
                    myAnimator.enabled = false;
                    myAnimator.SetBool("isClimbing", false);
                }
            }
        }
        else
        {
            myAnimator.enabled = true;
            myRigidbody.gravityScale = defaultGravity;
            myAnimator.SetBool("isClimbing", false);
        }
    }
}
