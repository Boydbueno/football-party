using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour 
{

    public List<AudioSetting> AudioSettings = new List<AudioSetting>();

    private AudioSource _audioSource;

    [Serializable]
    public class AudioSetting {
        public string key;
        public AudioClip audioClip;
    }

    public void Start() 
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(string key) 
    {
        AudioClip audioClip = AudioSettings.Find(item => item.key == key).audioClip;
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
