using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ProceduralSound
{

    [SerializeField]
    public float volume = 1; //the volume multiplier in relation to the rest of the music

    [SerializeField]
    public int channel; //a channel that'll play the music ( a pair of NotePlayer and an AudioSource, that is in the NotePlayer)

    [SerializeField]
    public NoteToFreq.NoteOnOctave note; //the note to be played

    [SerializeField]
    public int octave; //the note to be played

    //the time to play the note is defined by a fracti
    [SerializeField]
    public float playTimeNumerator = 1; 

    [SerializeField]
    public float playTimeDenominator = 1; 


    [SerializeField]
    public float fadeoutMult;

    public ProceduralSound() {

        volume = 1;
        channel = 0;
        note = NoteToFreq.NoteOnOctave.A;
        octave = 4;
        playTimeNumerator = 1;
        playTimeDenominator = 1;
        fadeoutMult = 0;

    }

    public float getPlayTimeFrac() {

        return playTimeNumerator / playTimeDenominator;

    }

    public ProceduralSound getSound(int index) {

        if (index == 0)
            return this;
        else
            return null;
    }

    
}
