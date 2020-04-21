using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float thrustSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;

    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private ParticleSystem fire;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        fire = GetComponentInChildren<ParticleSystem>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        var movement = thrustSpeed;

        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * movement);
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

        var rotation = Time.deltaTime * rotationSpeed;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidBody.transform.Rotate(Vector3.forward * rotation);
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            rigidBody.transform.Rotate(-Vector3.forward * rotation);
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DangerousElement>() != null)
        {
            Explode();
        }
    }

    private void Explode()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.velocity = Vector3.zero;
    }
}
