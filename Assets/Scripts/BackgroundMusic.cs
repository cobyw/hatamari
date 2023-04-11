using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    public float baseVolume = 1.0f;
    public float pauseVolumePercent = 0.15f;

    public static BackgroundMusic Instance
    {
        get; private set;
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        if (volume < 0 || volume > 1) //make sure we are passed a valid volume
        {
            return;
        }
        else
        {
            audioSource.volume = volume;
        }
    }

    public void PauseVolume(bool pausing)
    {
        //if we are pausing then use the base volume * the pause volume, otherwise we are unpausing and should use the base volume
        audioSource.volume = pausing ? baseVolume * pauseVolumePercent : baseVolume;
    }
}
