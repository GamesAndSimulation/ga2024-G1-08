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


    //these are used in the functions, no point in altering their values
    private double increment;

    private double phase; //the phase can be though as the X value for the wave, if the wave had a frequency of 1


    //this class exists because the normal implementations of LinkedList or List don't let us having freedom of iterating over the elements
    private class LinkedListSounds {

        public Node head; //the first node of the list

        public class Node {

            public SoundBeingPlayed sound;
            public Node next;
            public Node prev;

            public Node(SoundBeingPlayed sound, Node next) {

                this.sound = sound;
                this.next = next;
                this.prev = null;

            }

        }

        public LinkedListSounds() {

            head = null;

        }

        public void add(SoundBeingPlayed toAdd) {

            Node oldHead = head;
            head = new Node(toAdd, oldHead);

            if(oldHead != null)
                oldHead.prev = head;

        }

        public void remove(Node toRemove) {

            if(toRemove.next != null)
                toRemove.next.prev = toRemove.prev;

            if(toRemove.prev != null)
                toRemove.prev.next = toRemove.next;

            if(toRemove == head)
                head = null;

        }

    }


    struct SoundBeingPlayed {

        public double frequency;

        public float gain;
        public float fadeoutMult;

        public double startTime;
        public double duration;

    }

    LinkedListSounds soundsBeingPlayed;

    public void Start() {

        increment = 2 * Math.PI / sampling_frequency;
        soundsBeingPlayed = new LinkedListSounds();



    }

    public void playSound(ProceduralSound sound, double duration) {

        SoundBeingPlayed newSound;

        newSound.frequency = NoteToFreq.getFrequency(sound.note, sound.octave);

        newSound.gain = sound.volume;
        newSound.fadeoutMult = sound.fadeoutMult;

        newSound.duration = duration;
        newSound.startTime = AudioSettings.dspTime;

        soundsBeingPlayed.add(newSound);

    }


    //data is an array of floats, comprising of audio data to filter
    //In this case, the values of data are 0 because we're generating the sound
    public void OnAudioFilterRead(float[] data, int channels)
    {
        LinkedListSounds.Node currentNode;
        float value;

        //for each data for each channel
        for (int i = 0; i < data.Length / channels; i++) {

            phase = phase + increment; //the advance we make in the x axis of the funtion

            currentNode = soundsBeingPlayed.head;

            while (currentNode != null) {

                value = valueOfSound(currentNode.sound, phase); //the value in the y axis

                for (int channel = 0; channel < channels; channel++) {

                    data[channel + i * channels] += value; //the data of that channel is equal to the current value

                }

                currentNode = currentNode.next;

            }



        }

        currentNode = soundsBeingPlayed.head;

        while (currentNode != null) {

            //if sound has reached the end of its duration, remove it
            if (currentNode.sound.duration <= AudioSettings.dspTime - currentNode.sound.startTime)
                soundsBeingPlayed.remove(currentNode);

            //else, treat it
            else {

                currentNode.sound.gain = currentNode.sound.gain * (1 - currentNode.sound.fadeoutMult);

            }

            currentNode = currentNode.next;

        }

    }

    private static float valueOfSound(SoundBeingPlayed sound, double phase) {


        return (float)(sound.gain * (Math.Sin(phase * sound.frequency)));

    }


}
