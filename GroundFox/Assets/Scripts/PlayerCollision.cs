using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] float loadLevelDelay = 1f;
    [SerializeField] GameObject deathFX;

    private PlayerMovement playerMovement;
    private bool processingCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Collectible collectible = other.gameObject.GetComponent<Collectible>();
        if (collectible != null)
        {
            collectible.Capture();
        }
        else
        {
            StartDeathSequence();
        }
    }

    private void StartDeathSequence()
    {
        if (processingCollision)
        {
            return;
        }

        processingCollision = true;
        playerMovement.onPlayerDeath();
        deathFX.SetActive(true);

        Invoke("ReloadScene", loadLevelDelay);
    }

    private void ReloadScene() // String referenced
    {
        SceneManager.LoadScene(1);
    }
}
