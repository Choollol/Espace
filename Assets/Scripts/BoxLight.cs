using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxLight : MonoBehaviour
{
    public int shotSpeed;

    private Rigidbody2D rb;
    private GameObject player;
    private bool doMovePlayer;
    private float transformXPosOld;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (doMovePlayer && Mathf.Abs(rb.velocity.x) > 0.3f)
        {
            player.transform.position += new Vector3((transform.position.x - transformXPosOld) * 0.9f, 0);
        }
        transformXPosOld = transform.position.x;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (collision.transform.position.x < transform.position.x) 
            {
                rb.AddForce(Vector2.left * shotSpeed, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.right * shotSpeed, ForceMode2D.Impulse);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
            (transform.position.y + gameObject.GetComponent<SpriteRenderer>().size.y * transform.localScale.y / 2) < (collision.transform.position.y -
            collision.gameObject.GetComponent<SpriteRenderer>().size.y * collision.transform.localScale.y / 2 + 0.25))
        {
            doMovePlayer = true;
        }
        else if (collision.transform.position.y > transform.position.y && Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            collision.transform.position += new Vector3((transform.position.x - transformXPosOld) * 0.3f, 0);
            doMovePlayer = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            doMovePlayer = false;
        }
    }
}
