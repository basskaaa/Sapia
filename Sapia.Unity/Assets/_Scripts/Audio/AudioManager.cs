using Assets._Scripts.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource MusicSource;
    public AudioSource SfxSource;           

    public void PlaySound(float lowPitchRange, float highPitchRange, AudioClip clipToPlay, float volume)
    {
        SfxSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        SfxSource.PlayOneShot(clipToPlay);
        SfxSource.volume = volume;
    }

    public void PlaySound(AudioClip clipToPlay, float volume)
    {
        SfxSource.PlayOneShot(clipToPlay);
        SfxSource.volume = volume;
    }

    public void PlayMusic(AudioClip music, float volume)
    {
        MusicSource.volume = volume;
        MusicSource.clip = music;
        MusicSource.Play();
        MusicSource.loop = true;
    }
}
