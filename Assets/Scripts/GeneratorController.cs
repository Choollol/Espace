using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerController playerController;
    public AudioManager audioManager;

    public List<GameObject> groundChecks;
    public LayerMask groundLayer;

    public float transparentOpacity;
    public SpriteRenderer spriteRenderer;
    public bool canBePlaced = true;
    public bool isPlaced = false;
    public float circleGrowSpeed;

    private Color whiteTransparent;
    private Color redTransparent;

    private GameObject circle;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        whiteTransparent = new Color(255, 255, 255, transparentOpacity);
        redTransparent = new Color(255, 0, 0, transparentOpacity);

        circle = transform.GetChild(0).gameObject;
    }
    private void FixedUpdate()
    {
        if (!isPlaced && gameManager.isGameActive)
        {
            spriteRenderer.color = whiteTransparent;
            canBePlaced = true;
            transform.position += new Vector3(0.001f, 0);
        }
    }

    void Update()
    {
        if (gameManager.isGameActive && !gameManager.isGameOver)
        {
            if (!isPlaced)
            {
                transform.position = new Vector2(playerController.gameObject.transform.position.x, playerController.gameObject.transform.position.y + 1.05f);
                if (Input.GetButtonDown("Ability1") && !isPlaced)
                {
                    if (!spriteRenderer.enabled)
                    {
                        spriteRenderer.enabled = true;
                        spriteRenderer.color = whiteTransparent;
                        canBePlaced = false;
                    }
                    else
                    {
                        spriteRenderer.enabled = false;
                    }
                }
                transform.position -= new Vector3(0.001f, 0);
            }

            if (!isPlaced)
            {
                foreach (GameObject groundCheck in groundChecks)
                {
                    if (!Physics2D.OverlapCircle(groundCheck.transform.position, 0.14f, groundLayer))
                    {
                        canBePlaced = false;
                        spriteRenderer.color = redTransparent;
                    }
                }
                if (Mathf.Abs(playerController.GetComponent<Rigidbody2D>().velocity.y) > 0.001f)
                {
                    canBePlaced = false;
                    spriteRenderer.color = redTransparent;
                }
            }
            
            if (Input.GetButtonDown("Fire2") && canBePlaced && spriteRenderer.enabled && !isPlaced)
            {
                isPlaced = true;
                spriteRenderer.color = new Color(255, 255, 255, 1);
                playerController.isFrozen = true;
                StartCoroutine(ActivateGenerator());
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        bool isThisBottomLessColTop = (transform.position.y - spriteRenderer.size.y * transform.localScale.y / 2) < (collision.transform.position.y +
            collision.GetComponent<SpriteRenderer>().size.y * collision.transform.localScale.y / 2 - 0.25);
        if (!isPlaced && isThisBottomLessColTop && 
            !collision.CompareTag("Player") && !collision.CompareTag("Ground") && !collision.CompareTag("Bullet"))
        {
            canBePlaced = false;
            spriteRenderer.color = redTransparent;
        }
    }
    private IEnumerator ActivateGenerator()
    {
        audioManager.generatorActivated.Play();
        circle.gameObject.SetActive(true);
        circle.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.6f);
        circle.transform.localScale = Vector2.zero;
        StartCoroutine(GrowCircle());
        yield return new WaitForSeconds(4);
        gameManager.StartNextLevel();
    }
    private IEnumerator GrowCircle()
    {
        while (circle.transform.localScale.x < 8f)
        {
            circle.transform.localScale += new Vector3(circleGrowSpeed, circleGrowSpeed) * Time.deltaTime;
            circle.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.15f) * Time.deltaTime;
            if (!isPlaced)
            {
                yield break;
            }
            yield return null;
        }
        spriteRenderer.color = redTransparent;
        yield break;
    }
    public void ResetCircle()
    {
        StartCoroutine(ResetCircleSize());
    }
    private IEnumerator ResetCircleSize()
    {
        yield return new WaitForSeconds(0.1f);
        circle.transform.localScale = new Vector3(0.1f, 0.1f);
        circle.gameObject.SetActive(false);
        yield break;
    }
}
