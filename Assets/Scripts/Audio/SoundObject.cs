using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField] private AudioType audioType;
    [SerializeField] private AudioClip audioClip;

    public virtual void PlayAudio()
    {
        SoundMgr.Instance.TryPlaySound(audioType, audioClip);
    }
}
