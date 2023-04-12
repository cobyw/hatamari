using System.Collections;
using UnityEngine;

public enum AudioType
{
    Collect,
    Attack
}

[RequireComponent(typeof(AudioSource))]
public class SoundMgr : MonoBehaviour
{
    //timer to prevent different sounds from playing back to back/overlapping
    [SerializeField] private float overallSoundTimerMax = 5;

    [Header("Collect Sound Fields")]
    [SerializeField] private float collectSoundTimerMax = 5;
    [SerializeField] private int collectSoundChancePercent = 20;

    [Header("Attack Sound Fields")]
    [SerializeField] private float attackSoundTimerMax = 5;
    [SerializeField] private int attackSoundChancePercent = 20;

    [Header("Cached Components")]
    [SerializeField] private AudioSource audioSource;

    //variable for holding the chance a sound will occur
    private int soundChance;

    private bool canPlayAnySound;
    private bool canPlayCollectSound;
    private bool canPlayAttackSound;

    public static SoundMgr Instance
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
            DontDestroyOnLoad(this.gameObject);

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

        }
    }



    public bool TryPlaySound(AudioType audioType, AudioClip audioClip)
    {
        bool retval = false;

        if (audioType == AudioType.Collect)
        {
            if (canPlayCollectSound && canPlayAnySound)
            {
                soundChance = Random.Range(0, 100 / collectSoundChancePercent);
                //if the sound chance happens (picked 0 for the value as its guaranteed to be in all sets)
                if (soundChance == 0 || collectSoundChancePercent == 100)
                {
                    retval = true;
                    canPlayCollectSound = false;
                    StartCoroutine(StartTimer(audioType, collectSoundTimerMax));
                    canPlayAnySound = false;
                    StartCoroutine(StartAnySoundtimer());
                }
            }
        }
        else if (audioType == AudioType.Attack)
        {
            if (canPlayAttackSound && canPlayAnySound)
            {
                soundChance = Random.Range(0, 100 / attackSoundChancePercent);
                //if the sound chance happens (picked 0 for the value as its guaranteed to be in all sets)
                if (soundChance == 0 || attackSoundChancePercent == 100)
                {
                    retval = true;
                    canPlayAttackSound = false;
                    StartCoroutine(StartTimer(audioType, attackSoundTimerMax));
                    canPlayAnySound = false;
                    StartCoroutine(StartAnySoundtimer());
                }
            }
        }

        return retval;
    }

    private bool PlaySound(bool canPlaySoundType, float soundTypeChancePercent, float soundTimeTimerMax, AudioClip audioClip)
    {
        return false;
    }
    private IEnumerator StartTimer(AudioType audioType, float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        if (audioType == AudioType.Collect)
        {
            canPlayCollectSound = true;
        }
        else if (audioType == AudioType.Attack)
        {
            canPlayAttackSound = true;
        }
    }

    private IEnumerator StartAnySoundtimer()
    {
        yield return new WaitForSeconds(overallSoundTimerMax);
        canPlayAnySound = true;
    }
}