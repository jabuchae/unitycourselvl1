using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    const float SPEED = 1.0f;
    const float ROTATION_SPEED = 100.0f;

    private Rigidbody rigidbody;
    private AudioSource audioSource;
    private ParticleSystem fire;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        fire = GetComponentInChildren<ParticleSystem>();
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

        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * movement);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                fire.Play(true);
            }
        } else
        {
            audioSource.Stop();
            fire.Stop(true);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.transform.Rotate(Vector3.forward * rotation);
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            rigidbody.transform.Rotate(-Vector3.forward * rotation);
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }

    }
}
