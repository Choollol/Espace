using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float cameraMinY;
    public float cameraMaxY;
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (player.transform.position.y > cameraMinY && player.transform.position.y < cameraMaxY)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
    }
}
