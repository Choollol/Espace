using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    public GameManager gameManager;
    public AudioManager audioManager;

    public SpriteRenderer spriteRenderer;

    public bool isFrozen;

    public bool hasCanister;
    public GameObject canisterParticle;

    public LayerMask groundLayer;

    public float stepSoundInterval;

    private Vector3 velocity;
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject groundCheck;

    private float horizontalInput;

    public bool isGrounded;
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;
    private int extraJumps = 0;
    private int extraJumpCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundCheck = transform.GetChild(0).gameObject;
        canisterParticle = transform.GetChild(2).gameObject;

        InvokeRepeating("PlayStepSound", stepSoundInterval, stepSoundInterval);
    }

    private void FixedUpdate()
    {
        if (!isFrozen && gameManager.isGameActive)
        {
            Movement();
        }
    }
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Physics2D.OverlapCircle(groundCheck.transform.position, 0.17f, groundLayer))
        {
            isGrounded = true;
        }
        if (!isFrozen && gameManager.isGameActive)
        {
            Jump();
            animator.SetFloat("xSpeed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        }
        if (rb.velocity.y > jumpForce)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    private void Movement()
    {
        velocity = new Vector3(horizontalInput, 0) * speed * Time.deltaTime;

        rb.velocity = new Vector3(velocity.x, rb.velocity.y);
    }
    private void Jump()
    {
        float dt = Time.deltaTime;

        if (isGrounded || extraJumpCounter < extraJumps)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= dt;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= dt;
        }
        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpBufferCounter = 0;
            if (!isGrounded)
            {
                extraJumpCounter++;
            }
            isGrounded = false;
            audioManager.jump.Play();
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0;
        }
    }
    private void PlayStepSound()
    {
        if (Mathf.Abs(horizontalInput) > 0.1f && isGrounded)
        {
            audioManager.stepSound.Play();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Canister"))
        {
            hasCanister = true;
            Destroy(collision.gameObject);
            canisterParticle.SetActive(true);
            audioManager.canister.Play();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
