using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZObstacleControls : MonoBehaviour
{
    public float rotateSpeed;
    [SerializeField]
    float moveSpeed = 3f;
    [SerializeField]
    float distance = 1f;
    Vector3 pos;
    private void Start()
    {
        pos = transform.position;
    }
    void Update()
    {
        float newZ = Mathf.Sin(Time.time * moveSpeed) * distance + pos.z;
        transform.position = new Vector3(pos.x, pos.y, newZ);
        transform.Rotate(new Vector3(0, 30, 0) * (Time.deltaTime * rotateSpeed));
    }
}