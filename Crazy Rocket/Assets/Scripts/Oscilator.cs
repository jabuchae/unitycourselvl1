using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float period = 1;

    private float movementFactor = 0;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Protect against period = 0
        if (period <= Mathf.Epsilon)
        {
            return;
        }

        float cycle = Time.time / period;
        float sinWave = Mathf.Sin(cycle * Mathf.PI * 2);

        transform.position = initialPosition + movementVector * sinWave;
    }
}