using Assets._Scripts.Patterns;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource MusicSource;
    public AudioSource SfxSource;

    public AudioClipHolder[] sfxClips;

    public void PlaySound(AudioClip clipToPlay, float volume, float lowPitchRange, float highPitchRange)
    {
        SfxSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        SfxSource.PlayOneShot(clipToPlay);
        SfxSource.volume = volume;
    }

    public void PlaySound(AudioClip clipToPlay, float volume)
    {
        SfxSource.pitch = 1;
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
