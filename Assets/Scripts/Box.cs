using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject boxDestroyParticle;

    private AudioManager audioManager;
    void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    void Update()
    {
        
    }
    private void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) { return; }
        Instantiate(boxDestroyParticle, transform.position, transform.rotation);
        audioManager.boxNormalDestroy.Play();
        audioManager.boxNormalDestroy.pitch = (float)Random.Range(-20, 20) / 100 + 1;
    }
}
