using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveGlobe : MonoBehaviour
{
    private Vector3 center = Vector3.zero;
    private Vector3 verticalCenter = Vector3.zero;

    public float rotationalSpeed = 0.01f;
    public float verticalSpeed = 0.001f;
    public float radius = 45f;
    public float rotationalDecel = 0f;
    public float radiusIncrease = 0.001f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        verticalCenter.y += verticalSpeed;
        transform.Translate(Vector3.up * verticalSpeed, Space.World);
        transform.RotateAround(verticalCenter, Vector3.up, rotationalSpeed);
        transform.LookAt(center);
        if (rotationalSpeed > 0)
        {
            rotationalSpeed -= rotationalDecel;
        }
        transform.Translate(-transform.forward * radiusIncrease);
    }
}
