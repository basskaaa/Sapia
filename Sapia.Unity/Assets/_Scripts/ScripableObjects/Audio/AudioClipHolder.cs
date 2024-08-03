using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipHolder", menuName = "Data/AudioClipHolder")]

public class AudioClipHolder : ScriptableObject
{
    public float Volume = 0.5f;
    public float HighPitchRange = 0f;
    public float LowPitchRange = 0f;
    [Space (10)]
    public AudioClip AudioClip;
}
