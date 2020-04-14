using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    const float SPEED = 1.0f;
    const float ROTATION_SPEED = 100.0f;

    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        var rotation = Time.deltaTime * ROTATION_SPEED;
        var movement = SPEED;

        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * movement);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.transform.Rotate(Vector3.forward * rotation);
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            rigidbody.transform.Rotate(-Vector3.forward * rotation);
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

    }
}
