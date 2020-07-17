using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadSceneOne", 2f);
    }

    private void LoadSceneOne()
    {
        SceneManager.LoadScene(1);
    }
}
