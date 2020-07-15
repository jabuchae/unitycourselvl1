using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float thrustSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;

    [SerializeField] private AudioClip engineAudio;
    [SerializeField] private AudioClip destroyAudio;
    [SerializeField] private AudioClip winAudio;

    [SerializeField] private ParticleSystem engine;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private ParticleSystem success;

    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private static bool collisionActive = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessDebugInput();

        if (GameState.instance.IsPlayerActive()) {
            ProcessInput();
        } else
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void ProcessInput()
    {
        ProcessThrustInput();

        ProcessRotationInput();
    }

    private void ProcessRotationInput()
    {
        var rotation = Time.deltaTime * rotationSpeed;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidBody.transform.Rotate(Vector3.forward * rotation);
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidBody.transform.Rotate(-Vector3.forward * rotation);
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void ProcessThrustInput()
    {
        
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;

        if (Input.GetKey(KeyCode.Space))
        {
            Thrust();
        }
        else
        {
            DontThrust();
        }
    }
    private void Thrust()
    {
        var thrustingForce = thrustSpeed;
        rigidBody.AddRelativeForce(Vector3.up * thrustingForce);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineAudio);
            engine.Play(true);
        }
    }

    private void DontThrust()
    {
        audioSource.Stop();
        engine.Stop(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collisionActive)
        {
            return;
        }

        if (collision.gameObject.GetComponent<DangerousElement>() != null)
        {
            if (GameState.instance.IsPlayerActive())
            {
                Explode();
            }
        }

        var winLevel = collision.gameObject.GetComponent<Level>();
        if (winLevel != null)
        {
            if (GameState.instance.IsPlayerActive())
            {
                Win(winLevel);
            }
        }
    }

    private void Explode()
    {
        DontThrust();
        audioSource.PlayOneShot(destroyAudio);
        explosion.Play(true);
        Level.current.Die();
    }

    private void Win(Level level)
    {
        this.transform.parent = level.transform;
        DontThrust();
        success.Play(true);
        level.Win();
        audioSource.PlayOneShot(winAudio);
    }

    private void ProcessDebugInput()
    {
        if (!Debug.isDebugBuild)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            Level.current.LoadNextLevel();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            Rocket.collisionActive = !Rocket.collisionActive;
            Debug.Log("Collision Active: " + Rocket.collisionActive);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            LevelAnimation.skipAnimations = !LevelAnimation.skipAnimations;
            Debug.Log("Skip animations: " + LevelAnimation.skipAnimations);
        }
    }
}
