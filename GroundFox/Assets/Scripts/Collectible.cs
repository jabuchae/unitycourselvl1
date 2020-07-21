using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] int score = 10;

    private AudioSource collectibleAudio;

    private void Start()
    {
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;

        collectibleAudio = gameObject.GetComponent<AudioSource>();
    }

    public void Capture()
    {
        ScoreBoard.instance.ScorePoints(score);
        collectibleAudio.Play();
        var renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.enabled = false;
        var collider = gameObject.GetComponent<SphereCollider>();
        collider.enabled = false;
        Destroy(gameObject, 2f);
    }
}
