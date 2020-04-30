using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public static Level current;

    private float timeLeft = 0f;
    private float endLevelTime = 2.0f;
    private float startLevelTime = 1.0f;

    void Start()
    {
        current = this;
        timeLeft = startLevelTime;
    }

    public void Win()
    {
        timeLeft = endLevelTime;
        GameState.instance.SetStatus(GameState.Status.WinLevel);
    }

    void Update()
    {
        if (GameState.instance.GetStatus() == GameState.Status.WinLevel)
        {
            UpdateForWinLevel();
            return;
        }

        if (GameState.instance.GetStatus() == GameState.Status.StartLevel)
        {
            UpdateForStartLevel();
            return;
        }
    }

    private void UpdateForWinLevel()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            LoadNextLevel();
        }
    }

    private void UpdateForStartLevel()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            GameState.instance.SetStatus(GameState.Status.Playing);
        }
    }

    private void LoadNextLevel()
    {
        timeLeft = startLevelTime;
        GameState.instance.SetStatus(GameState.Status.StartLevel);
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    public void ReloadLevel()
    {
        timeLeft = startLevelTime;
        GameState.instance.SetStatus(GameState.Status.StartLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
