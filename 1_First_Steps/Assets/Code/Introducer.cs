using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introducer : MonoBehaviour
{
    public float speed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        print("Hi, my name is Red Car");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Transform>().Translate(new Vector3(-speed, 0, 0));
        print("I'm at:" + gameObject.GetComponent<Transform>().position);
    }
}
