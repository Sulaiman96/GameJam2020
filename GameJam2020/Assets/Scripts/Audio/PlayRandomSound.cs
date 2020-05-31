using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
     
    [SerializeField] private AudioClip[] audioClips = null;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
    }

    public void PlayRandomClip()
    {
        audioSource.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Length - 1)];
        audioSource.PlayOneShot(audioSource.clip);
    }
}
