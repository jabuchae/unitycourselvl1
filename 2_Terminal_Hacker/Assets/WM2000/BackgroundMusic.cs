using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    static BackgroundMusic backgroundMusic;

    public AudioClip normalMusic;
    public AudioClip cameraMusic;
    public AudioClip creditsMusic;

    public AudioSource mainAudioSource;
    public AudioSource secondaryAudioSource;

    private const float switchTime = 1f;
    private float switchingTimePassed = 0;

    // Different states of the game
    enum State { playing, switching }

    // Game State
    private State currentState;

    private void Awake()
    {
        if (backgroundMusic == null) { backgroundMusic = this; } // Be the one
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.switching)
        {
            performSwitch();
        }
    }

    static public void PlayNormalMusic()
    {
        backgroundMusic.switchToClip(backgroundMusic.normalMusic);
    }

    static public void PlayCameraMusic()
    {
        backgroundMusic.switchToClip(backgroundMusic.cameraMusic);
    }

    static public void PlayCreditsMusic()
    {
        backgroundMusic.switchToClip(backgroundMusic.creditsMusic);
    }

    private void switchToClip(AudioClip clip)
    {
        secondaryAudioSource.clip = clip;
        secondaryAudioSource.volume = 0;
        secondaryAudioSource.Play();
        currentState = State.switching;
    }

    private void performSwitch()
    {
        switchingTimePassed += Time.deltaTime;

        if (switchingTimePassed > switchTime)
        {
            switchingTimePassed = 0;
            mainAudioSource.volume -= 0.01f;
            secondaryAudioSource.volume += 0.01f;

            if (mainAudioSource.volume <= 0)
            {
                AudioSource aux = mainAudioSource;
                mainAudioSource = secondaryAudioSource;
                secondaryAudioSource = aux;
                secondaryAudioSource.Stop();
                currentState = State.playing;
            }
        }
    }
}
