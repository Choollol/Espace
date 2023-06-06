using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public int destroyLimit;
    public GameObject bulletCollisionParticle;

    private Rigidbody2D rb;

    private PlayerController playerController;
    private AudioManager audioManager;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
        transform.position += transform.right * 0.15f;

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x) > destroyLimit || Mathf.Abs(transform.position.y) > destroyLimit)
        {
            PlayDestroy();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            PlayDestroy();
            if (collision.gameObject.CompareTag("Box"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Generator") &&
            !collision.gameObject.CompareTag("Canister"))
        {
            PlayDestroy();
            if (collision.gameObject.CompareTag("Box"))
            {
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.CompareTag("Box Reverse"))
            {
                if (collision.GetComponent<Rigidbody2D>().gravityScale == 1)
                {
                    collision.GetComponent<Rigidbody2D>().gravityScale = -1;
                }
                else
                {
                    collision.GetComponent<Rigidbody2D>().gravityScale = 1;
                }
            }
            else if (collision.gameObject.CompareTag("Box Heavy") && playerController.hasCanister)
            {
                Destroy(collision.gameObject);
                audioManager.boxHeavyDestroy.Play();
            }
            playerController.hasCanister = false;
        }
    }
    private void PlayDestroy()
    {
        Destroy(this.gameObject);
        Instantiate(bulletCollisionParticle, transform.position, transform.rotation);
    }
}
