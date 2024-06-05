using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SimpleAudioMixerComponent : MonoBehaviour
{

    public static SimpleAudioMixerComponent instance { get; private set; }

    private void Awake() {

        if (instance == null)
            instance = this;


        DontDestroyOnLoad(this);

    }

    [SerializeReference] private AudioMixer audioMixer;


    [Range(0.00001f, 1)] public float masterVolume;
    [Range(0.00001f, 1)] public float sfxVolume;
    [Range(0.00001f, 1)] public float musicVolume;

    // Update is called once per frame
    void Update()
    {

        audioMixer.SetFloat("MasterVolume",Mathf.Log10(masterVolume) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);


    }
}
