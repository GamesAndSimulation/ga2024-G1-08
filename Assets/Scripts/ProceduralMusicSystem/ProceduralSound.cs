using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ProceduralSound : ScriptableObject, ProceduralMusicParcel
{

    [SerializeField]
    public float volume; //the volume multiplier in relation to the rest of the music

    [SerializeField]
    public int channel; //a channel that'll play the music ( a pair of NotePlayer and an AudioSource, that is in the NotePlayer)

    [SerializeField]
    public float waitTime; //time to wait for the note, in fractions

    [SerializeField]
    public NoteToFreq.Note note; //the note to be played

    [SerializeField]
    public float playTime; //Time to play the note, in fractions

    [SerializeField]
    public bool fadeout;

    public ProceduralSound getSound(int index) {

        if (index == 0)
            return this;
        else
            return null;
    }
}
