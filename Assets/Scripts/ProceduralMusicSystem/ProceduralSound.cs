using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProceduralSound
{
    public NoteToFreq.Note note; //the note to be played

    public float volume; //the volume

    public int timeToPlayMili; //time for the note to play in milliseconds

    public int channel; //a channel that'll play the music ( a pair of NotePlayer and an AudioSource, that is in the NotePlayer)

    public int timeToWaitForPlay; //when are we supposed to play this sound in relation to the previous sound played


}
