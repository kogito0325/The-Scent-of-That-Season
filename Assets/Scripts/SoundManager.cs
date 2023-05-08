using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource sfxPlayer;
    public AudioClip[] clips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void PlayClip(int clipIdx)
    {
        sfxPlayer.clip = clips[clipIdx];
        sfxPlayer.Play();
    }
}
