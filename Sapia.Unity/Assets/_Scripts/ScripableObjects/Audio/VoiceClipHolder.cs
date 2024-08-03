using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Voice Clip Holder", menuName = "Data/Voice Clip Holder")]
public class VoiceClipHolder : ScriptableObject
{
    public AudioClip[] VoiceLine;
    public float[] VoiceLineDelay;
}
