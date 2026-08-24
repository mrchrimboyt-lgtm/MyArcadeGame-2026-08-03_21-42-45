using UnityEngine;
using Random = System.Random;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public sound[] Sounds; //stores an array of music

    public void Play(string name) //plays the sound file 
    {
        sound s = Array.Find(Sounds, sound => sound.name == name);//finds sound in list
        if (s.source == null)//If sound is not been loaded yet. Load new audio source
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
        s.source.Play(); //play sound
    } 
    
}
