using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    [SerializeField] private bool isPlayOnStart = false;
    [SerializeField] private bool isTriggerCollider = false;
    [SerializeField] private bool isEvent = false;

    [SerializeField] private VoiceClipHolder voiceHolder;

    private bool hasBeenPlayed = false;

    private void Start()
    {
        if (isPlayOnStart && !hasBeenPlayed) 
        {
            VoiceManager.Instance.AddVoiceLineToQueue(voiceHolder);
            Debug.Log("Trigger");
            hasBeenPlayed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isTriggerCollider && !hasBeenPlayed)
        {
            VoiceManager.Instance.AddVoiceLineToQueue(voiceHolder);
            hasBeenPlayed = true;
        }
    }

    public void EventVoiceLine()
    {
        if (isEvent && !hasBeenPlayed) 
        {
            VoiceManager.Instance.AddVoiceLineToQueue(voiceHolder);
            hasBeenPlayed = true;
        }
    }
}
