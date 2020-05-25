using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public static Level current;
    [SerializeField] private float endLevelTime = 2.0f;
    [SerializeField] private float startLevelTime = 1.0f;
    [SerializeField] private GameObject platform;
    [SerializeField] private Material launchpadMaterial;

    private float timeLeft = 0f;
    private float dyingTime = 1.5f;

    void Start()
    {
        current = this;
        GameState.instance.SetStatus(GameState.Status.StartLevel);
        timeLeft = startLevelTime;
    }

    public void Win()
    {
        timeLeft = endLevelTime;
        platform.GetComponent<Renderer>().material = launchpadMaterial;
        GameState.instance.SetStatus(GameState.Status.WinLevel);
    }

    public void Die()
    {
        timeLeft = dyingTime;
        GameState.instance.SetStatus(GameState.Status.Dying);
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

        if (GameState.instance.GetStatus() == GameState.Status.Dying)
        {
            UpdateForDying();
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

    private void UpdateForDying()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            ReloadLevel();
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
