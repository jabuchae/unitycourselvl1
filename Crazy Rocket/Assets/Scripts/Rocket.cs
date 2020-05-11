﻿using System.Collections;
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

    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private ParticleSystem fire;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        fire = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
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
            fire.Play(true);
        }
    }

    private void DontThrust()
    {
        audioSource.Stop();
        fire.Stop(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
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
                winLevel.Win();
                audioSource.PlayOneShot(winAudio);
            }
        }
    }

    private void Explode()
    {
        audioSource.PlayOneShot(destroyAudio);
        Level.current.Die();
    }
}
