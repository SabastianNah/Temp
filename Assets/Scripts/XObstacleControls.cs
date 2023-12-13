using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XObstacleControls : MonoBehaviour
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
        float newX = Mathf.Sin(Time.time * moveSpeed) * distance + pos.x;
        transform.position = new Vector3(newX, pos.y, pos.z);
        transform.Rotate(new Vector3(0, 30, 0) * (Time.deltaTime * rotateSpeed));
    }
}
