using UnityEngine;
using System.Reflection;

public class Terminal : MonoBehaviour
{
    DisplayBuffer displayBuffer;
    InputBuffer inputBuffer;

    static Terminal primaryTerminal;

    private bool inputEnabled = true;

    public AudioClip normalMusic;
    public AudioClip cameraMusic;
    public AudioClip creditsMusic;

    private AudioSource audioSource;

    static public void PlayNormalMusic()
    {
        primaryTerminal.audioSource.clip = primaryTerminal.normalMusic;
        primaryTerminal.audioSource.Play();
    }

    static public void PlayCameraMusic()
    {
        primaryTerminal.audioSource.clip = primaryTerminal.cameraMusic;
        primaryTerminal.audioSource.Play();
        primaryTerminal.audioSource.volume -= 0.10f;
    }

    static public void PlayCreditsMusic()
    {
        primaryTerminal.audioSource.clip = primaryTerminal.creditsMusic;
        primaryTerminal.audioSource.Play();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (primaryTerminal == null) { primaryTerminal = this; } // Be the one
        inputBuffer = new InputBuffer();
        displayBuffer = new DisplayBuffer(inputBuffer);
        inputBuffer.onCommandSent += NotifyCommandHandlers;
    }

    public string GetDisplayBuffer(int width, int height)
    {
        return displayBuffer.GetDisplayBuffer(Time.time, width, height);
    }

    public void ReceiveFrameInput(string input)
    {
        inputBuffer.ReceiveFrameInput(input);
    }

    public static void ClearScreen()
    {
        primaryTerminal.displayBuffer.Clear();
    }

    public static void WriteLine(string line)
    {
        primaryTerminal.displayBuffer.WriteLine(line);
    }

    public static void WriteChar(string character)
    {
        primaryTerminal.displayBuffer.WirteChar(character);
    }

    public static void EnableInput()
    {
        primaryTerminal.inputEnabled = true;
    }

    public static void DisableInput()
    {
        primaryTerminal.inputEnabled = false;
    }

    public static bool InputEnabled()
    {
        return primaryTerminal.inputEnabled;
    }

    public void NotifyCommandHandlers(string input)
    {
        var allGameObjects = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour mb in allGameObjects)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var targetMethod = mb.GetType().GetMethod("OnUserInput", flags);
            if (targetMethod != null)
            {
                object[] parameters = new object[1];
                parameters[0] = input;
                targetMethod.Invoke(mb, parameters);
            }
        }
    }
}