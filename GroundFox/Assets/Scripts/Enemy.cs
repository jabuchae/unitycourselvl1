using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float destroyDelay = 0.4f;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private GameObject deathFX;
    [SerializeField] private int scorePerHit = 1;
    [SerializeField] private int scorePerKill = 7;
    [SerializeField] private int life = 3;

    private void Start()
    {
        AddNonTriggerBoxCollider();
    }

    private void AddNonTriggerBoxCollider()
    {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        life--;
        ScoreHit();
        if (life <= 0)
        {
            StartDeathAnimation();
            ScoreKill();
        }
    }

    private void ScoreKill()
    {
        ScoreBoard.instance.ScorePoints(scorePerKill);
    }

    private void ScoreHit()
    {
        ScoreBoard.instance.ScorePoints(scorePerHit);
    }

    private void StartDeathAnimation()
    {
        CreateDeathEffect();
        DestroyEnemy();   
    }

    private void CreateDeathEffect()
    {
        GameObject explosion = Instantiate<GameObject>(deathFX, transform.position, Quaternion.identity);
        explosion.transform.parent = spawnParent;
        explosion.SetActive(true);
    }

    private void DestroyEnemy() // Referenced by string
    {
        Destroy(gameObject);
    }
}
