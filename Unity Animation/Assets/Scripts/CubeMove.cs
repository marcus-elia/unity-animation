using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    private float rotationalXAccel;
    private float rotationalYAccel;
    private float rotationalZAccel;

    private Vector3 rotationalVelocity = Vector3.zero;

    public static float maxAccel = 0.0001f;
    public static float movementSpeed = 0.001f;
    // Start is called before the first frame update
    void Start()
    {
        rotationalXAccel = Random.Range(-maxAccel, maxAccel);
        rotationalYAccel = Random.Range(-maxAccel, maxAccel);
        rotationalZAccel = Random.Range(-maxAccel, maxAccel);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationalVelocity);
        rotationalVelocity.x += rotationalXAccel;
        rotationalVelocity.y += rotationalYAccel;
        rotationalVelocity.z += rotationalZAccel;

        if (transform.position.y > 0)
        {
            transform.Translate(Vector3.down * movementSpeed, Space.World);
        }
    }
}
