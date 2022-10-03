using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerGameplay : MonoBehaviour
{
    public static SoundManagerGameplay instance {get; private set;}
    public AudioSource efxSource;
    public AudioSource musicSource;
    public AudioSource backgroundNoiseSource;

    void Awake()
    {
        if (instance != null) {
            GameObject.Destroy(this);
        } else {
            instance = this;
        }
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;

        efxSource.Play ();
    }
}
