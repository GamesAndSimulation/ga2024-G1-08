using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralMusicPlayer : MonoBehaviour
{

    //the channels that'll play the music, no channel can play notes at the same time
    public List<NotePlayer> channels;


    public ProceduralMusic musicToPlay;

    private int parcelPlayingIndex; //the index of the next parcel to play (or that we're currently playing)
    private int soundPlayingNowIndex; //the index of the next sound to play in the next or current parcel

    private ProceduralSound soundPlayingNow; //next sound to play

    private float timeToWait; //the time we need to wait to play the next note
    


    [SerializeField]
    private bool toRepeat = true;


    public void Start() {

        parcelPlayingIndex = 0;
        soundPlayingNowIndex = 0;

    }

    public void Update() {
       
       //if we wait enough to play the next sound
       if (timeToWait <= 0) {

            switchToNextSound();
            channels[soundPlayingNow.channel].playSound(soundPlayingNow.note,
                                            timeFromMusic(musicToPlay.beatsPerMinute, soundPlayingNow.getPlayTimeFrac()) - musicToPlay.noteIntervalDelayMili, //the time to play the music, the delay is used here
                                            soundPlayingNow.fadeout);


            timeToWait = timeFromMusic(musicToPlay.beatsPerMinute, soundPlayingNow.getPlayTimeFrac());



        } else {

            timeToWait -= Time.deltaTime * 1000; //the miliseconds we're waiting
       
       }

    }

    private void switchToNextSound() {

        soundPlayingNowIndex++;
        soundPlayingNow = musicToPlay.musicParcels[parcelPlayingIndex].getSound(soundPlayingNowIndex);

        //if this parcel does not have more sounds
        if(soundPlayingNow == null) {

            //switch to next parcel and reset soundToPlayIndex
            soundPlayingNowIndex = 0;
            parcelPlayingIndex++;

            //if there is no next parcel
            if (parcelPlayingIndex >= musicToPlay.musicParcels.Count) {

                if (!toRepeat)
                    gameObject.SetActive(false);

                parcelPlayingIndex = 0;

            }

            soundPlayingNow = musicToPlay.musicParcels[parcelPlayingIndex].getSound(soundPlayingNowIndex);
        
        }

    }


    //calculates the actual time in miliseconds of a note
    public static int timeFromMusic(int bpm, float timeFraction) {


        //60000 is the normal beats per minute
        //4 is because we refer to quarters
        return (int) (60000 * 4 * timeFraction / bpm);

    }


}
