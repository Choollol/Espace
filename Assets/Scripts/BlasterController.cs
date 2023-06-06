using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlasterController : MonoBehaviour
{
    public PlayerController playerController;
    public SpriteRenderer spriteRenderer;
    public GameManager gameManager;

    public GameObject bulletPrefab;

    public AudioManager audioManager;

    public int ammoCount;
    public TextMeshProUGUI ammoCountText;

    void Start()
    {
    }

    void Update()
    {
        if (!playerController.isFrozen && gameManager.isGameActive)
        {
            Rotate();
            Shoot();
        }
        ammoCountText.text = ammoCount.ToString();
    }
    private void Rotate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position) * Quaternion.Euler(0, 0, 90);
        if (mousePos.x < playerController.gameObject.transform.position.x)
        {
            playerController.spriteRenderer.flipX = true;
            spriteRenderer.flipY = true;
        }
        else
        {
            playerController.spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }
    }
    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && ammoCount > 0)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            audioManager.blasterFire.Play();
            audioManager.blasterFire.pitch = (float)Random.Range(-20, 20) / 100 + 1;
            ammoCount--;
            playerController.canisterParticle.SetActive(false);
        }
    }
    public void SetAmmoCount(int ammo)
    {
        ammoCount = ammo;
    }
}
