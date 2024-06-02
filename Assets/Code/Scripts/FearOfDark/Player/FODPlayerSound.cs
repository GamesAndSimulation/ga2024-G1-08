using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public class FODPlayerSound  
{

    [SerializeReference] public SFXSoundComponent soundComponent;

    [Space(), SerializeField] public Vector2 pitchInterval;

    [Space(), SerializeField] public Vector2 volumeInterval;


    public FODPlayerSound(SFXSoundComponent soundComponent, Vector2 pitchInterval, Vector2 volumeInterval) {

        this.soundComponent = soundComponent;
        this.pitchInterval = pitchInterval;
        this.volumeInterval = volumeInterval;

    }

    public void setup(float pitchPer=0, float volumePer=0) {

        updateSound(pitchPer, volumePer);
        soundComponent.PlaySound();
    }

    public void updateSound(float pitchPer, float volumePer) {

        soundComponent.audioSource.pitch = pitchInterval[0] + (pitchInterval[1] - pitchInterval[0]) * pitchPer;
        soundComponent.audioSource.volume = volumeInterval[0] + (volumeInterval[1] - volumeInterval[0]) * volumePer;


    }


}