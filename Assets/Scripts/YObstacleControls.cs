using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YObstacleControls : MonoBehaviour
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
        float newY = Mathf.Sin(Time.time * moveSpeed) * distance + pos.y;
        transform.position = new Vector3(pos.x, newY, pos.z);
        transform.Rotate(new Vector3(0, 30, 0) * (Time.deltaTime * rotateSpeed));
    }       
}
