using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackgroundSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    private void OnTriggerEnter(Collider other)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
