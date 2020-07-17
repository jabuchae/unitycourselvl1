using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjects : MonoBehaviour
{
    private void Awake()
    {
        int staticObjectsCount = FindObjectsOfType<StaticObjects>().Length;
        if (staticObjectsCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
