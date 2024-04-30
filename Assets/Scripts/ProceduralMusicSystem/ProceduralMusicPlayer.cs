using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralMusicPlayer : MonoBehaviour
{

    //the channels that'll play the music, no channel can play notes at the same time
    public List<NotePlayer> channels;


    public ProceduralMusic musicToPlay;

    private int nextParcelToPlayIndex; //the index of the next parcel to play (or that we're currently playing)
    private int nextSoundToPlayIndex; //the index of the next sound to play in the next or current parcel

    private ProceduralSound nextToPlay; //next sound to play

    private float timeWaitingToPlay; //the time we already waited to play the next sound
    


    [SerializeField]
    private bool toRepeat = true;


    public void Start() {

        nextParcelToPlayIndex = 0;
        nextSoundToPlayIndex = 0;
        switchToNextSound();

    }

    public void Update() {
       
       //if we wait enough to play the next sound
       if (timeFromMusic(musicToPlay.beatsPerMinute, nextToPlay.waitTime) <= timeWaitingToPlay) {
       
       
           playSounds();
       
       
       } else {
       
           timeWaitingToPlay += Time.deltaTime * 1000; //the miliseconds we're waiting
       
       }

    }

    private void switchToNextSound() {

        nextToPlay = musicToPlay.musicParcels[nextParcelToPlayIndex].getSound(nextSoundToPlayIndex); //the first time this is called, the index is already 0

        //if this parcel does not have more sounds
        if(nextToPlay == null) {

            //switch to next parcel and reset soundToPlayIndex
            nextSoundToPlayIndex = 0;
            nextParcelToPlayIndex++;

            //if there is no next parcel
            if (nextParcelToPlayIndex >= musicToPlay.musicParcels.Count) {

                if (!toRepeat)
                    gameObject.SetActive(false);

                nextParcelToPlayIndex = 0;

            }

            nextToPlay = musicToPlay.musicParcels[nextParcelToPlayIndex].getSound(nextSoundToPlayIndex);
        
        }

    }

    /**
     * 
     * Plays the next batch of sounds that are waiting
     */
    private void playSounds() {

        //we already know that we can play the next sound
        do {

            Debug.Log("Playing sound at " + nextSoundToPlayIndex + " at batch " + nextParcelToPlayIndex);

            channels[nextToPlay.channel].playSound(nextToPlay.note, 
                timeFromMusic(musicToPlay.beatsPerMinute, nextToPlay.getPlayTimeFrac()) - musicToPlay.noteIntervalDelayMili, //the time to play the music, the delay is used here
                nextToPlay.fadeout);

            nextSoundToPlayIndex++;
            timeWaitingToPlay = 0;

            switchToNextSound();

        
            //this while is so if the next sound had a time of 0, we can play it in the same frame
        } while (timeFromMusic(musicToPlay.beatsPerMinute, nextToPlay.waitTime) <= timeWaitingToPlay);

  
    }



    //calculates the actual time in miliseconds of a note
    public static int timeFromMusic(int bpm, float timeFraction) {


        //60000 is the normal beats per minute
        //4 is because we refer to quarters
        return (int) (60000 * 4 * timeFraction / bpm);

    }


}
