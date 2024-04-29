using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct ProceduralSound
{
    [SerializeField]
    public NoteToFreq.Note note; //the note to be played

    [SerializeField]
    public float volume; //the volume

    [SerializeField]
    public int timeToPlayMili; //time for the note to play in milliseconds

    [SerializeField]
    public int channel; //a channel that'll play the music ( a pair of NotePlayer and an AudioSource, that is in the NotePlayer)

    [SerializeField]
    public int timeToWaitForPlay; //when are we supposed to play this sound in relation to the previous sound played


}
