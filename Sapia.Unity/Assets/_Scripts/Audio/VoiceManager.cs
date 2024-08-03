using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets._Scripts.Patterns;

public class VoiceManager : Singleton<VoiceManager>
{
    private List<VoiceClipHolder> voiceHolders = new List<VoiceClipHolder>();

    private AudioSource voiceSource;

    private bool isPlaying = false;

    protected override void Awake()
    {
        voiceSource = GetComponent<AudioSource>();
        base.Awake();
    }

    public void AddVoiceLineToQueue(VoiceClipHolder voiceHolder)
    {
        voiceHolders.Add(voiceHolder);

        if (!isPlaying)
        {
            StartCoroutine(PlayVoiceQueue(voiceHolder));
        }
    }

    private IEnumerator PlayVoiceQueue(VoiceClipHolder voiceHolder)
    {
        isPlaying = true;

        for (int i = 0; i < voiceHolder.VoiceLine.Length; i++)
        {
            voiceSource.clip = voiceHolder.VoiceLine[i];
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
            //Debug.Log(voiceHolder.VoiceLine[i].name);

            if (voiceHolder.VoiceLineDelay.Length > 0)
            {
                yield return new WaitForSeconds(voiceHolder.VoiceLineDelay[i]);
                //Debug.Log(voiceHolder.VoiceLineDelay[i]);
            }
        }
        voiceHolders.Remove(voiceHolder);
        isPlaying = false;

        if (voiceHolders.Count > 0) 
        {
            StartCoroutine(PlayVoiceQueue(voiceHolders[0]));
        }
    }
}
