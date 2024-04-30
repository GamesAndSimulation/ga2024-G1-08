using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ProceduralSound
{

    [SerializeField]
    public float volume; //the volume multiplier in relation to the rest of the music

    [SerializeField]
    public int channel; //a channel that'll play the music ( a pair of NotePlayer and an AudioSource, that is in the NotePlayer)

    [SerializeField]
    public float waitTime; //time to wait for the note, in fractions

    [SerializeField]
    public NoteToFreq.Note note; //the note to be played

    
    //the time to play the note is defined by a fracti
    [SerializeField]
    public int playTimeNumerator; 

    [SerializeField]
    public int playTimeDenumerator; 


    [SerializeField]
    public bool fadeout;

    public float getPlayTimeFrac() {

        return playTimeNumerator / playTimeDenumerator;

    }

    public ProceduralSound getSound(int index) {

        if (index == 0)
            return this;
        else
            return null;
    }
}
