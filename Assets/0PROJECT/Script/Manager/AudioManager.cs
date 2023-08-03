// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioPlay;
    public AudioSource soundPlay;
    float timer;

    private void Awake()
    {
        // DontDestroyOnLoad(this.gameObject);
    
    }

   private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlaySound, OnPlaySound);
        EventManager.AddHandler(GameEvent.OnPlaySoundVolume, OnPlaySoundVolume);
        EventManager.AddHandler(GameEvent.OnPlaySoundPitch, OnPlaySoundPitch);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlaySound, OnPlaySound);
        EventManager.RemoveHandler(GameEvent.OnPlaySoundVolume, OnPlaySoundVolume);
        EventManager.RemoveHandler(GameEvent.OnPlaySoundPitch, OnPlaySoundPitch);
    }


    private void OnPlaySound(object value)
    {
        audioPlay.clip = Resources.Load<AudioClip>((string)value);
        audioPlay.PlayOneShot(audioPlay.clip);
    }

    private void OnPlaySoundVolume(object value, object volume)
    {
        audioPlay.volume = (float)volume;
        audioPlay.clip = Resources.Load<AudioClip>((string)value);
        audioPlay.PlayOneShot(audioPlay.clip);
    }

    private void OnPlaySoundPitch(object value, object volume, object pitch)
    {
        audioPlay.volume = (float)volume;
        audioPlay.pitch = (float)pitch;

        audioPlay.clip = Resources.Load<AudioClip>((string)value);
        audioPlay.PlayOneShot(audioPlay.clip);
    }

    private void OnPlaySoundBg(object value, object volume)
    {
        soundPlay.volume = (float)volume;

        soundPlay.clip = Resources.Load<AudioClip>((string)value);
        soundPlay.PlayOneShot(soundPlay.clip);
    }

  








}
