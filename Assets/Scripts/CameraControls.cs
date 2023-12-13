using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{

    public Transform player;

    //distance between player and camera
    public Vector3 cameraOffset;

    //Camera rotation
    public float smoothFactor = 0.5f;

    public bool looking = false;
    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = player.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        //rotating camera
        if (looking)
        {
            transform.LookAt(player);
        }
    }
}
