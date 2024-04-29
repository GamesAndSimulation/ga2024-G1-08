using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMusicPlayer : MonoBehaviour
{

    //the channels that'll play the music
    public List<NotePlayer> channels;


    public ProceduralMusic musicToPlay;

    private int timeWaitingToPlay;
    private int nextSoundToPlay;


    public void Start() {

        timeWaitingToPlay = 0;
        nextSoundToPlay = 0;

    }

    public void Update() {

        ProceduralSound nextToPlay = musicToPlay.sounds[nextSoundToPlay];
        if (nextToPlay.timeToWaitForPlay <= timeWaitingToPlay) {

            channels[nextToPlay.channel].playSound(nextToPlay.note, nextToPlay.timeToPlayMili);
        
        }



    }


}
