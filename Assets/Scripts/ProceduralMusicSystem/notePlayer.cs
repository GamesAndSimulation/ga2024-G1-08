using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notePlayer : MonoBehaviour
{

    // un-optimized version
    public double frequency = 440;
    private double increment;
    private double phase;
    private double sampling_frequency = 48000;

    public float gain;
    public float volume = 0.1f;

    private AudioSource audioSource;

    private bool canPlayNote = true;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnAudioFilterRead(float[] data, int channels)
    {
        // update increment in case frequency has changed
        increment = frequency * 2 * Math.PI / sampling_frequency;

        for (var i = 0; i < data.Length; i = i + channels){

            phase = phase + increment;
            // this is where we copy audio data to make them “available” to Unity
            data[i] = (float)(gain * Math.Sin(phase));

            // if we have stereo, we copy the mono data to each channel
            if (channels == 2) data[i] = data[i];
            if (phase > 2 * Math.PI) phase = 0;
        
        }
    }

    IEnumerator playForMiliseconds(int milliseconds)
    {
        canPlayNote = false;

        yield return new WaitForSeconds(milliseconds / 1000f); // Convert milliseconds to seconds

        audioSource.Stop();

        canPlayNote= true;
    }


}
