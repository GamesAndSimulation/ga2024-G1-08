using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralMusicPlayer : MonoBehaviour
{

    //the channels that'll play the music
    public List<NotePlayer> channels;


    public ProceduralMusic musicToPlay;

    private float timeWaitingToPlay;
    private int nextSoundToPlay;


    [SerializeField]
    private bool toRepeat = true;


    public void Start() {

        timeWaitingToPlay = 0;
        nextSoundToPlay = 0;

    }

    public void Update() {

        //the next sound to play
        ProceduralSound nextToPlay = musicToPlay.sounds[nextSoundToPlay];

        //if we wait enough to play the next sound
        if (timeFromMusic(musicToPlay.beatsPerMinute, nextToPlay.waitTime) <= timeWaitingToPlay) {

            playSounds();


        } else {

            timeWaitingToPlay += Time.deltaTime * 1000;

        }

    }

    /**
     * 
     * Plays the next batch of sounds that are waiting
     */
    private void playSounds() {

        //the next sound to play
        ProceduralSound nextToPlay = musicToPlay.sounds[nextSoundToPlay]; ;

        do {

            channels[nextToPlay.channel].playSound(nextToPlay.note, timeFromMusic(musicToPlay.beatsPerMinute, nextToPlay.playTime));
            nextSoundToPlay++;
            timeWaitingToPlay = 0;

            if (nextSoundToPlay >= musicToPlay.sounds.Count) {

                if (toRepeat) {

                    nextSoundToPlay = 0;
                    break;

                } else
                    gameObject.SetActive(false);

            }


        } while (timeFromMusic(musicToPlay.beatsPerMinute, nextToPlay.waitTime) <= timeWaitingToPlay);

  
    }



    //calculates the actual time in miliseconds of a note
    public static int timeFromMusic(int bpm, float timeFraction) {

        //60000 is the normal beats per minute
        //4 is because we refer to quarters
        return (int) (60000 * 4 * timeFraction / bpm);

    }


}
