using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundPlayer;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{

    public enum SoundFunctions { Base, DecayingHarmonic, DecayingHarmonic2, DecayingHarmonic3, DecayingHarmonicOffset, DecayingHarmonicTimeVariation, PianoApprox };
    public enum WaveFunctions { Sign, Cos, Saw, Square };
    public enum GainFunctions { LinearExit, CosExit, SinChangeCosExit};


    public SoundFunctions soundFunction = SoundFunctions.Base;
    public WaveFunctions waveFunction = WaveFunctions.Sign;
    public GainFunctions gainFuntion = GainFunctions.LinearExit;
    

    private double sampling_frequency = 48000; //frequency of sample creation (from continuous signals to discrete signals)


    //these are used in the functions, no point in altering their values
    private double increment;

    private double phase; //the phase can be though as the X value for the wave, this goes from 0 to 1. This means that  wave funtions must have their domain compensated accordingly


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

    delegate float ValueOfSound(SoundBeingPlayed sound, double phase, double timePassed); //this declares a type, "ValueOfSound", that describes functions with a certain return type and list of parameters
    delegate double BaseWaveFunction(double x);
    delegate float GainFunction(SoundBeingPlayed sound, double timePlayed);

    struct SoundBeingPlayed {

        public double frequency;

        public float gain;
        public float fadeoutMult;

        public double startTime;
        public double duration;

        public ValueOfSound value;
        public BaseWaveFunction baseWaveFunction;
        public GainFunction gainFunction;



    }

    LinkedListSounds soundsBeingPlayed;

    public void Start() {

        increment = 1 / sampling_frequency;
        soundsBeingPlayed = new LinkedListSounds();



    }

    public void playSound(ProceduralSound sound, double duration) {

        SoundBeingPlayed newSound;

        newSound.frequency = NoteToFreq.getFrequency(sound.note, sound.octave);

        newSound.gain = sound.volume;
        newSound.fadeoutMult = sound.fadeoutMult;

        newSound.duration = duration;
        newSound.startTime = AudioSettings.dspTime;

        newSound.value = defineFuntionOfSound();
        newSound.baseWaveFunction = defineWaveFuntionOfSound();
        newSound.gainFunction = defineGainFunctionOfSound();
        

        soundsBeingPlayed.add(newSound);

    }


    //data is an array of floats, comprising of audio data to filter
    //In this case, the values of data are 0 because we're generating the sound
    public void OnAudioFilterRead(float[] data, int channels)
    {
        LinkedListSounds.Node currentNode;
        double currentTIme = AudioSettings.dspTime;
        float value;

        //for each data for each channel
        for (int i = 0; i < data.Length / channels; i++) {

            phase = phase + increment; //the advance we make in the x axis of the funtion

            currentNode = soundsBeingPlayed.head;

            while (currentNode != null) {

                value = currentNode.sound.value(currentNode.sound, phase, currentTIme - currentNode.sound.startTime); //the value in the y axis

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
            //else {

                //currentNode.sound.gain = currentNode.sound.gain * (1 - currentNode.sound.fadeoutMult);

            //}

            currentNode = currentNode.next;

        }

    }

    #region ValueFunctions

    private ValueOfSound defineFuntionOfSound() {

        switch (soundFunction) {

            case SoundFunctions.DecayingHarmonic:

                return new ValueOfSound(valueOfSoundDecayingHarmonics);

            case SoundFunctions.DecayingHarmonic2:


                return new ValueOfSound(valueOfSoundDecayingHarmonicsV2);

            case SoundFunctions.DecayingHarmonic3:

                return new ValueOfSound(valueOfSoundDecayingHarmonicsV3);


            case SoundFunctions.DecayingHarmonicOffset:

                return new ValueOfSound(valueOfSoundDecayingHarmonicsOffsets);

            case SoundFunctions.DecayingHarmonicTimeVariation:

                return new ValueOfSound(valueOfSoundDecayingHarmonicsTimeVariation);

            case SoundFunctions.PianoApprox:

                return new ValueOfSound(valueOfSoundPianoApproximation);



            default:
                return new ValueOfSound(valueOfSound);

        }

    }

    private float valueOfSound(SoundBeingPlayed sound, double phase, double timePassed) {


        return (float)(sound.gainFunction(sound, timePassed) * 
            (sound.baseWaveFunction(phase * sound.frequency)));

    }

    private float valueOfSoundDecayingHarmonics(SoundBeingPlayed sound, double phase, double timePassed) {


        return (float)(
            (sound.gainFunction(sound, timePassed) / 2) * (sound.baseWaveFunction(phase * sound.frequency)) +
            (sound.gainFunction(sound, timePassed) / 4) * (sound.baseWaveFunction(phase * sound.frequency * 2)) +
            (sound.gainFunction(sound, timePassed) / 16) * (sound.baseWaveFunction(phase * sound.frequency * 3)) +
            (sound.gainFunction(sound, timePassed) / 16) * (sound.baseWaveFunction(phase * sound.frequency * 4)) +
            (sound.gainFunction(sound, timePassed) / 16) * (sound.baseWaveFunction(phase * sound.frequency * 5)) +
            (sound.gainFunction(sound, timePassed) / 16) * (sound.baseWaveFunction(phase * sound.frequency * 6))
            );

    }

    private float valueOfSoundDecayingHarmonicsV2(SoundBeingPlayed sound, double phase, double timePassed) {


        return (float)(
            (sound.gainFunction(sound, timePassed) / 2) * (sound.baseWaveFunction(phase * sound.frequency)) +
            (sound.gainFunction(sound, timePassed) / 4) * (sound.baseWaveFunction(phase * sound.frequency * 2)) +
            (sound.gainFunction(sound, timePassed) / 8) * (sound.baseWaveFunction(phase * sound.frequency * 3)) +
            (sound.gainFunction(sound, timePassed) / 32) * (sound.baseWaveFunction(phase * sound.frequency * 4)) +
            (sound.gainFunction(sound, timePassed) / 64) * (sound.baseWaveFunction(phase * sound.frequency * 5)) +
            (sound.gainFunction(sound, timePassed) / 128) * (sound.baseWaveFunction(phase * sound.frequency * 6))
            );

    }

    private float valueOfSoundDecayingHarmonicsV3(SoundBeingPlayed sound, double phase, double timePassed) {


        return (float)(
            (sound.gainFunction(sound, timePassed) / 2) * (sound.baseWaveFunction(phase * sound.frequency)) +
            (sound.gainFunction(sound, timePassed) / 4) * (sound.baseWaveFunction(phase * sound.frequency * 2)) +
            (sound.gainFunction(sound, timePassed) / 8) * (sound.baseWaveFunction(phase * sound.frequency * 3)) +
            (sound.gainFunction(sound, timePassed) / 32) * (sound.baseWaveFunction(phase * sound.frequency * 4)) +
            (sound.gainFunction(sound, timePassed) / 64) * (sound.baseWaveFunction(phase * sound.frequency * 5)) +
            (sound.gainFunction(sound, timePassed) / 128) * (sound.baseWaveFunction(phase * sound.frequency * 6)) +
            (sound.gainFunction(sound, timePassed) / 254) * (sound.baseWaveFunction(phase * sound.frequency) * 7) +
            (sound.gainFunction(sound, timePassed) / 512) * (sound.baseWaveFunction(phase * sound.frequency * 8)) +
            (sound.gainFunction(sound, timePassed) / 1024) * (sound.baseWaveFunction(phase * sound.frequency * 9)) +
            (sound.gainFunction(sound, timePassed) / 2048) * (sound.baseWaveFunction(phase * sound.frequency * 10)) +
            (sound.gainFunction(sound, timePassed) / 4096) * (sound.baseWaveFunction(phase * sound.frequency * 11)) +
            (sound.gainFunction(sound, timePassed) / 8192) * (sound.baseWaveFunction(phase * sound.frequency * 12))
            );

    }

    private float valueOfSoundDecayingHarmonicsOffsets(SoundBeingPlayed sound, double phase, double timePassed) {


        return (float)(
            (sound.gainFunction(sound, timePassed) / 4) * (sound.baseWaveFunction(phase * sound.frequency)) +
            (sound.gainFunction(sound, timePassed) / 8) * (sound.baseWaveFunction(phase * sound.frequency * 2 - increment )) +
            (sound.gainFunction(sound, timePassed) / 8) * (sound.baseWaveFunction(phase * sound.frequency * 3 - increment * 2 )) +
            (sound.gainFunction(sound, timePassed) / 8) * (sound.baseWaveFunction(phase * sound.frequency * 4 - increment * 4 )) +
            (sound.gainFunction(sound, timePassed) / 8) * (sound.baseWaveFunction(phase * sound.frequency * 5 - increment * 8 )) +
            (sound.gainFunction(sound, timePassed) / 8) * (sound.baseWaveFunction(phase * sound.frequency * 6 - increment * 16)) +
            (sound.gainFunction(sound, timePassed) / 16) * (sound.baseWaveFunction(phase * sound.frequency * 7 - increment * 32))
            );

    }

    private float valueOfSoundDecayingHarmonicsTimeVariation(SoundBeingPlayed sound, double phase, double timePlayed) {


        return (float)(

            (sound.gainFunction(sound, timePlayed) / 2) * (sound.baseWaveFunction(phase * sound.frequency)) * (0.8 * Math.Sin(timePlayed) + 0.2 * Math.Cos(timePlayed)) +
            (sound.gainFunction(sound, timePlayed) / 4) * (sound.baseWaveFunction(phase * sound.frequency * 2)) * (0.2 * Math.Sin(timePlayed) + 0.8 * Math.Cos(timePlayed)) +
            (sound.gainFunction(sound, timePlayed) / 8) * (sound.baseWaveFunction(phase * sound.frequency * 3)) * (0.8 * Math.Sin(timePlayed) + 0.2 * Math.Cos(timePlayed)) +
            (sound.gainFunction(sound, timePlayed) / 16) * (sound.baseWaveFunction(phase * sound.frequency * 4)) * (0.2 * Math.Sin(timePlayed) + 0.8 * Math.Cos(timePlayed))

            );

    }

    private float valueOfSoundPianoApproximation(SoundBeingPlayed sound, double phase, double timePlayed) {


        double toReturn =

            sound.baseWaveFunction(    sound.frequency * phase) * Math.Exp(-0.0004 * 2 * Math.PI * sound.frequency * phase) +
            sound.baseWaveFunction(2 * sound.frequency * phase) * Math.Exp(-0.0004 * 2 * Math.PI * sound.frequency * phase) / 2 +
            sound.baseWaveFunction(3 * sound.frequency * phase) * Math.Exp(-0.0004 * 2 * Math.PI * sound.frequency * phase) / 4 +
            sound.baseWaveFunction(4 * sound.frequency * phase) * Math.Exp(-0.0004 * 2 * Math.PI * sound.frequency * phase) / 8 +
            sound.baseWaveFunction(5 * sound.frequency * phase) * Math.Exp(-0.0004 * 2 * Math.PI * sound.frequency * phase) / 16 +
            sound.baseWaveFunction(6 * sound.frequency * phase) * Math.Exp(-0.0004 * 2 * Math.PI * sound.frequency * phase) / 32;

        //return sound.gainFunction(sound, timePlayed) * (float)(Math.Pow(toReturn, 3) + toReturn); 
        return 10 * (float)(Math.Pow(toReturn, 3) + toReturn);
    }

    #endregion


    #region WaveFuntions

    private BaseWaveFunction defineWaveFuntionOfSound() {

        switch (waveFunction) {

            case WaveFunctions.Cos:
                return new BaseWaveFunction(cosFunction);

            case WaveFunctions.Saw:
                return new BaseWaveFunction(sawFuntion);

            case WaveFunctions.Square:
                return new BaseWaveFunction(squareWave);


            default:
                return sinFunction;
        }

    }

    private double sinFunction(double x) {

        return Math.Sin(2 * Math.PI * x);

    }

    private double cosFunction(double x) {

        return Math.Cos(2 * Math.PI * x);

    }

    private double sawFuntion(double x) {

        return (((x + 0.5) % 1) - 0.5) * 2;

    }

    private double squareWave(double x) {

        return (x % 1) < 0.5 ? 1 : -1;

    }


    #endregion

    #region GainFunctions

    private GainFunction defineGainFunctionOfSound() {

        switch (gainFuntion) {

            case GainFunctions.CosExit:
                return cosExit;

            case GainFunctions.SinChangeCosExit: 
                return sinChangeCosExit;

            default:
                return new GainFunction(linearExit);

        }

    }


    private float linearExit(SoundBeingPlayed sound, double timePlayed) {


        return (float)(1 - (timePlayed / sound.duration));


    }

    private float cosExit(SoundBeingPlayed sound, double timePlayed) {


        return (float) Math.Cos( Math.PI * (timePlayed / sound.duration) / 2 );


    }

    private float sinChangeCosExit(SoundBeingPlayed sound, double timePlayed) {


        return (float) ( 0.5 + 0.5 * (Math.Cos(Math.PI * (timePlayed / sound.duration))) + (0.25 * Math.Sin(  Math.PI * 2 * (timePlayed /sound.duration)))  );


    }


    #endregion

}
