using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackgroundSound : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private GameObject bgMusic;
    private bool hasChanged = false;
    private void Start()
    {
        bgMusic = GameObject.FindGameObjectWithTag("BackgroundMusic");
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player"))
            return;

        var bGAudio = bgMusic.GetComponent<AudioSource>();
        if (!bGAudio || hasChanged)
            return;

        hasChanged = true;
        bGAudio.clip = clip;
        bGAudio.Play();
    }
}
