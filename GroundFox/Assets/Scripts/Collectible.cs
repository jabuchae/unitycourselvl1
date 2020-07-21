using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] int score = 10;

    private void Start()
    {
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
    }

    public void Capture()
    {
        ScoreBoard.instance.ScorePoints(score);
        Destroy(gameObject);
    }
}
