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

    public void playSound(NoteToFreq.NoteOnOctave note, int octave, bool fadeout) {

        if(fadeout)
            playSoundFadeout(note, octave);

        else
            playSound(note, octave);

    }

    public void playSound(NoteToFreq.NoteOnOctave note, int octave)
    {
        
       frequency = NoteToFreq.getFrequency(note, octave);
       gain = volume;

       audioSource.Play();

    }

    //Faded out the sound
    public void playSoundFadeout(NoteToFreq.NoteOnOctave note, int octave) {

        playSound(note, octave);

    }


    //data is an array of floats, comprising of audio data
    public void OnAudioFilterRead(float[] data, int channels)
    {

        for(int channel = 0; channel < channels; channel++) {

            onAudioFilterReadFOrChannel(data, channel, channels);

        }

    }

    public void onAudioFilterReadFOrChannel(float[] data, int channel, int channels) {


        // update increment in case frequency has changed
        increment = frequency * 2 * Math.PI / sampling_frequency;

        int nextData = channel;

        while (nextData < data.Length) {

            phase = phase + increment; //the advance we make in the x axis of the funtion

            // this is where we copy audio data to make them “available” to Unity

            data[nextData] = (float)(gain * Math.Sin(phase)); //the Y valye we have in the current phase we're in

            if (phase > 2 * Math.PI) phase = phase - 2 * Math.PI; //the sin funtion repeats for every 2 PI

            nextData += channels;


        }


    }

}
