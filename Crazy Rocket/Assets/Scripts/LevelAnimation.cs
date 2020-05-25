using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 moveVector;
    [SerializeField] private float completeTime;
    [SerializeField] private float delay;
    [SerializeField] private GameState.Status animateWhen;

    private float movementElapsed;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        movementElapsed = 0f;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.instance.GetStatus() == animateWhen)
        {
            if (delay >= 0)
            {
                delay -= Time.deltaTime;
            }
            else
            {
                MoveObject(Time.deltaTime);
            }
        }
    }

    private void MoveObject(float factor)
    {
        if (movementElapsed >= 1) { return; }

        movementElapsed += factor / completeTime;
        transform.position = initialPosition + moveVector * movementElapsed;
    }
}
