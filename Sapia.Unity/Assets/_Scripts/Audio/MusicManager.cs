using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClipHolder[] ambientMusic;

    public void PlayMusic()
    {
        AudioManager.Instance.PlayMusic(ambientMusic[0].AudioClip, ambientMusic[0].Volume);
    }
}
