using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{
    public ParticleSystem dust;
    private float horizontal;
    [Header("Horizontal Movement")]
    public float normalSpeed = 8f;
    public float crouchSpeed = 4f;
    public float acceleration;
    public float decceleration;
    public float velPower;
    public float friction;
    [Space(3)]
    [Header("Jump Movement")]
    public float jumpingPower = 16f;
    public float jumpCutMultiplier = .5f;
    public float fallGravityMultiplier = 2;
    public float minJump = .2f;
    [Header("Coyote and Buffer Time")]
    public float jumpCoyoteTime = .15f;
    public float jumpBufferTime = .1f;
    private float minJumpTimer;
    private bool isFacingRight = true;
    private bool crouching = false;
    private bool paused = false;
    static float speed;
    private float lastGroundedTime = 0;
    private float lastJumpTime = 0;
    private bool isJumping = false;
    private bool jumpInputReleased = false;
    private float gravityScale;
    private bool cutJump = false;
    [Space(3)]
    [Header("Object References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        //default speed to normalSpeed variable
        speed = normalSpeed;
        //get and store default gravityScale from rigidbody set in dropdown in Unity
        gravityScale = rb.gravityScale;

        minJumpTimer = minJump;
    }
    void Update()
    {
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;
        minJumpTimer -= Time.deltaTime;
        //Check if input in Unity defined as Fire1 is pressed
      //  if (Input.GetButtonDown("Fire1"))
      //  {
       //     //toggle paused bool
      //      paused = !paused;
       // }

        if (!paused)
        {
            if (IsGrounded())
            {
                //sets lastGroundedTime = jumpCoyoteTime; when lastGroundedTime is > 0, it means the player can jump
                //jumpCoyoteTime gives a "buffer" to jump, which allows jumping slightly after falling off ledge for better
                //feel
                lastGroundedTime = jumpCoyoteTime;
                isJumping = false;
                if (lastJumpTime > 0)
                {
                    jump();
                }
            }
            //Get range from -1 to 1 of horizontal input (in Unity, default mapped to 'a' and 'd' || left and right arrows)
            horizontal = Input.GetAxisRaw("Horizontal");

            //Check if falling (rb.velocity is negative)
            if (rb.velocity.y < 0)
            {
                if (isJumping)
                {
                    isJumping = false;
                }
                //Increase gravity after reaching apex of jump, creates "bouncy" feel
                rb.gravityScale = gravityScale * fallGravityMultiplier;
            }
            else
            {
                rb.gravityScale = gravityScale;
            }

            //Check if input for jump (in Unity settings) is pressed and character is colliding with ground
            if (Input.GetButtonDown("Jump"))
            {
                cutJump = false;
                lastJumpTime = jumpBufferTime;
                jump();
            }

            //Check if input for jump has been released
            if (Input.GetButtonUp("Jump"))
            {
                cutJump = true;
            }

            jumpReleased();

            crouch();

            Flip();

            //decrements time since last grounded and last jump

        }
    }

    private void crouch()
    {
        //checks if "down" is pressed: default in Unity settings is 's' || down arrow
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            crouching = true;
            speed = crouchSpeed;
        }
        else
        {
            crouching = false;
            speed = normalSpeed;
        }
    }
    private void jump()
    {
        minJumpTimer = minJump;
        if (lastGroundedTime > 0 && !isJumping)
        {
            CreateDust();
            rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
            lastGroundedTime = 0;
            lastJumpTime = 0;
            isJumping = true;
            jumpInputReleased = false;
        }
    }

    private void jumpReleased()
    {
        if (isJumping && minJumpTimer < 0 && cutJump == true)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
        jumpInputReleased = true;
    }

    private void FixedUpdate()
    {

        //get targeted movement
        float targetSpeed = horizontal * speed;
        //calculate difference between current speed and target
        float speedDif = targetSpeed - rb.velocity.x;
        //update acceleration
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        //use acceleration to find velocity (movement)
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        //apply horizontal forece to player based on movement
        rb.AddForce(movement * Vector2.right);
        if (horizontal != 0 && !isJumping && lastGroundedTime > 0) {
            CreateDust();
        }
        //Fakes a friction amount so that it is easier to tweak in Unity: when movement (velocity) is particularly small...
        if (lastGroundedTime > 0 && Mathf.Abs(horizontal) < 0.01f)
        {
            //set an amount of friction to be applied based on whether friction or velocity is currently smaller
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(friction));
            //update with correct direction
            amount *= Mathf.Sign(rb.velocity.x);

            //apply friction as a force to player, in opposite direction of player
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        if (!IsGrounded())
        {
            //keep isJumping set correctly every fixed frame
            isJumping = true;
        }

    }

    private bool IsGrounded()
    {
        //checks if the player is reasonably close to ground
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        //check if player has moved in direction opposite of current facing
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            //set isFacingRight to opposite
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            //flip current sprite by scaling x-axis by -1
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered");
        Scene scene = SceneManager.GetActiveScene();
        int nextLevelBuildIndex = scene.buildIndex + 1;
        if (nextLevelBuildIndex == 13)
        {
            nextLevelBuildIndex = 14;
        }
        Debug.Log(nextLevelBuildIndex);
        SceneManager.LoadScene(nextLevelBuildIndex);
    }

    private void CreateDust() {
        dust.Play();
    }
}
