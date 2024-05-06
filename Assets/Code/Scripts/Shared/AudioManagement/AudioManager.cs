using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public static AudioManager instance;


    [SerializeField]
    private AudioSource audioSourcePrefab;


    public void Awake() { //called when an enabled script is being loaded
        
        if(instance == null) {

            instance = this;
            DontDestroyOnLoad(gameObject); //this object is not destroyed when loading a different scene


        } else 
            Destroy(gameObject); //if there was already an instance of an object with this script, we destroy this (there should only be one)


    }

    public void playClip(AudioClip clip, AudioSource source, float volume) {

        source.clip = clip;
        source.volume = volume;
        source.Play();


    }
    
    //this method should be used by entities that can be destroied while making a sound, but their sound is to be prolonged after their death
    public void playClip(AudioClip clip, Transform spawnTransform, float volume) { 
    
        AudioSource audioSource = Instantiate(audioSourcePrefab, transform);

        playClip(clip, audioSource, volume);
    
        Destroy(audioSource, clip.length);
    
    }

}
