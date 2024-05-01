using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundPlayer;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{

    public enum SoundFunctions { Base, DecayingHarmonic, DecayingHarmonic2, DecayingHarmonic3, DecayingHarmonicOffset};

    public SoundFunctions soundFunction = SoundFunctions.Base;

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

    delegate float ValueOfSound(SoundBeingPlayed sound, double phase); //this declares a type, "ValueOfSound", that describes functions with a certain return type and list of parameters

    struct SoundBeingPlayed {

        public double frequency;

        public float gain;
        public float fadeoutMult;

        public double startTime;
        public double duration;

        public ValueOfSound value;



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

        switch (soundFunction) {

            case SoundFunctions.DecayingHarmonic:

                newSound.value = new ValueOfSound(valueOfSoundDecayingHarmonics);
                break;

            case SoundFunctions.DecayingHarmonic2:


                newSound.value = new ValueOfSound(valueOfSoundDecayingHarmonicsV2);
                break;

            case SoundFunctions.DecayingHarmonic3:

                newSound.value = new ValueOfSound(valueOfSoundDecayingHarmonicsV3);
                break;


            case SoundFunctions.DecayingHarmonicOffset:

                newSound.value = new ValueOfSound(valueOfSoundDecayingHarmonicsOffsets);
                break;

            default:
                newSound.value = new ValueOfSound(valueOfSound);
                break;
        
        }

        

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

                value = currentNode.sound.value(currentNode.sound, phase); //the value in the y axis

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

    private float valueOfSound(SoundBeingPlayed sound, double phase) {


        return (float)(sound.gain * 
            (Math.Sin(phase * sound.frequency)));

    }

    private float valueOfSoundDecayingHarmonics(SoundBeingPlayed sound, double phase) {


        return (float)(
            (sound.gain / 2) * (Math.Sin(phase * sound.frequency)) +
            (sound.gain / 4) * (Math.Sin(phase * sound.frequency * 2)) +
            (sound.gain / 16) * (Math.Sin(phase * sound.frequency * 3)) +
            (sound.gain / 16) * (Math.Sin(phase * sound.frequency * 4)) +
            (sound.gain / 16) * (Math.Sin(phase * sound.frequency * 5)) +
            (sound.gain / 16) * (Math.Sin(phase * sound.frequency * 6))
            );

    }

    private float valueOfSoundDecayingHarmonicsV2(SoundBeingPlayed sound, double phase) {


        return (float)(
            (sound.gain / 2) * (Math.Sin(phase * sound.frequency)) +
            (sound.gain / 4) * (Math.Sin(phase * sound.frequency * 2)) +
            (sound.gain / 8) * (Math.Sin(phase * sound.frequency * 3)) +
            (sound.gain / 32) * (Math.Sin(phase * sound.frequency * 4)) +
            (sound.gain / 64) * (Math.Sin(phase * sound.frequency * 5)) +
            (sound.gain / 128) * (Math.Sin(phase * sound.frequency * 6))
            );

    }

    private float valueOfSoundDecayingHarmonicsV3(SoundBeingPlayed sound, double phase) {


        return (float)(
            (sound.gain / 2) * (Math.Sin(phase * sound.frequency)) +
            (sound.gain / 4) * (Math.Sin(phase * sound.frequency * 2)) +
            (sound.gain / 8) * (Math.Sin(phase * sound.frequency * 3)) +
            (sound.gain / 32) * (Math.Sin(phase * sound.frequency * 4)) +
            (sound.gain / 64) * (Math.Sin(phase * sound.frequency * 5)) +
            (sound.gain / 128) * (Math.Sin(phase * sound.frequency * 6)) +
            (sound.gain / 254) * (Math.Sin(phase * sound.frequency) * 7) +
            (sound.gain / 512) * (Math.Sin(phase * sound.frequency * 8)) +
            (sound.gain / 1024) * (Math.Sin(phase * sound.frequency * 9)) +
            (sound.gain / 2048) * (Math.Sin(phase * sound.frequency * 10)) +
            (sound.gain / 4096) * (Math.Sin(phase * sound.frequency * 11)) +
            (sound.gain / 8192) * (Math.Sin(phase * sound.frequency * 12))
            );

    }

    private float valueOfSoundDecayingHarmonicsOffsets(SoundBeingPlayed sound, double phase) {


        return (float)(
            (sound.gain / 4) * (Math.Sin(phase * sound.frequency)) +
            (sound.gain / 8) * (Math.Sin(phase * sound.frequency * 2 - increment )) +
            (sound.gain / 8) * (Math.Sin(phase * sound.frequency * 3 - increment * 2 )) +
            (sound.gain / 8) * (Math.Sin(phase * sound.frequency * 4 - increment * 4 )) +
            (sound.gain / 8) * (Math.Sin(phase * sound.frequency * 5 - increment * 8 )) +
            (sound.gain / 8) * (Math.Sin(phase * sound.frequency * 6 - increment * 16)) +
            (sound.gain / 16) * (Math.Sin(phase * sound.frequency * 7 - increment * 32))
            );

    }

}
