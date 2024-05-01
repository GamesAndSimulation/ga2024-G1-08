using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{

    // un-optimized version
    public double frequency = 440;

    private double sampling_frequency = 48000; //frequency of sample creation (from continuous signals to discrete signals)

    public float gain;
    public float volume = 0.1f;
    public float fadeoutMult = 0f;

    //these are used in the functions, no point in altering their values
    private double increment;
    private double phase; //the phase can be though as the X value for the wave, if the wave had a frequency of 1

    struct SoungBeingPlayed {

        public float gain;
        public float fadeoutMult;
        
        public double increment;
        public double phase;

    }

    List<SoungBeingPlayed> soundsBeingPlayed;

    public void playSound(ProceduralSound sound) {

        gain = sound.volume;
        frequency = NoteToFreq.getFrequency(sound.note, sound.octave);
        fadeoutMult = sound.fadeoutMult;
    }


    //data is an array of floats, comprising of audio data to filter
    //In this case, the values of data are 0 because we're generating the sound
    public void OnAudioFilterRead(float[] data, int channels)
    {

        // update increment in case frequency has changed
        increment = frequency * 2 * Math.PI / sampling_frequency;

        float value;

        //for each data for each channel
        for (int i = 0; i < data.Length / channels; i++) {

            phase = phase + increment; //the advance we make in the x axis of the funtion
            value = (float)(gain * Math.Sin(phase)); //the value in the y axis

            for (int channel = 0; channel < channels; channel++) {

                data[channel + i * channels] = value; //the data of that channel is equal to the current value

            }

        }

        gain = gain * (1 - fadeoutMult);


    }


}
