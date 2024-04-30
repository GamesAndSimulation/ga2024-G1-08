using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NotePlayer : MonoBehaviour
{

    // un-optimized version
    public double frequency = 440;

    private double sampling_frequency = 48000;

    public float gain;
    public float volume = 0.1f;

    private AudioSource audioSource;

    //these are used in the functions, no point in altering their values
    private double increment;
    private double phase;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playSound(NoteToFreq.NoteOnOctave note, int octave, int miliDuration, bool fadeout) {

        if(fadeout)
            playSoundFadeout(note, octave, miliDuration);

        else
            playSound(note, octave, miliDuration);

    }

    public void playSound(NoteToFreq.NoteOnOctave note, int octave, int miliDuration)
    {
        
       frequency = NoteToFreq.getFrequency(note, octave);
       gain = volume;

       audioSource.Play();

       StartCoroutine(playForMiliseconds(miliDuration));

    }

    //Faded out the sound
    public void playSoundFadeout(NoteToFreq.NoteOnOctave note, int octave, int miliDuration) {

        playSound(note, octave, miliDuration / 2);

        for(int i = 9; i >= 0;  i--) {

            gain = volume * (i / 10);
            playSound(note, octave, miliDuration / (2 * 10));


        }

    }


    //data is an array of floats, comprising of audio data
    public void OnAudioFilterRead(float[] data, int channels)
    {
        // update increment in case frequency has changed
        increment = frequency * 2 * Math.PI / sampling_frequency;

        for (var i = 0; i < data.Length; i = i + channels){

            phase = phase + increment; //the advance we make in the x axis of the funtion

            // this is where we copy audio data to make them “available” to Unity

            data[i] = (float)(gain * Math.Sin(phase)); //the Y valye we have in the current phase we're in

            // if we have stereo, we copy the mono data to each channel
            if (channels == 2) data[i] = data[i];

            if (phase > 2 * Math.PI) phase = 0;
        
        }
    }

    IEnumerator playForMiliseconds(int milliseconds)
    {

        yield return new WaitForSeconds(milliseconds / 1000f); // Convert milliseconds to seconds

        audioSource.Stop();

    }


}
