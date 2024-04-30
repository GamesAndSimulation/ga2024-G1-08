using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteToFreq : MonoBehaviour
{
    public enum Note { 
        
        /*          +0    +1     +2   +3      +4  +5    +6     +7    +8    +9    +10    +11   */
        
        /*0*/       C0, CSharp0, D0, DSharp0, E0, F0, FSharp0, G0, GSharp0, A0, ASharp0, B0, // sub contra

        /*12*/      C1, CSharp1, D1, DSharp1, E1, F1, FSharp1, G1, GSharp1, A1, ASharp1, B1, // contra
        
        /* 24*/     C2, CSharp2, D2, DSharp2, E2, F2, FSharp2, G2, GSharp2, A2, ASharp2, B2, // great
        
        /* 36*/     C3, CSharp3, D3, DSharp3, E3, F3, FSharp3, G3, GSharp3, A3, ASharp3, B3, // small

        /* 48*/     C4, CSharp4, D4, DSharp4, E4, F4, FSharp4, G4, GSharp4, A4, ASharp4, B4, // one line
        
        /* 60*/     C5, CSharp5, D5, DSharp5, E5, F5, FSharp5, G5, GSharp5, A5, ASharp5, B5, // second line
        
        /*72*/      C6, CSharp6, D6, DSharp6, E6, F6, FSharp6, G6, GSharp6, A6, ASharp6, B6, // third line
        
        /* 84*/     C7, CSharp7, D7, DSharp7, E7, F7, FSharp7, G7, GSharp7, A7, ASharp7, B7, // fourth line
        
        /* 96*/     C8, CSharp8, D8, DSharp8, E8, F8, FSharp8, G8, GSharp8, A8, ASharp8, B8  // fith line

    }

    public enum NoteOnOctave {

        C, CSharp, D, DSharp, E, F, FSharp, G, GSharp, A, ASharp, B

    }


    private static float[] frequenciesOfFirstOctaveNotes = new float[]
        {
              16.35f,   17.32f,   18.35f,   19.45f,   20.60f,   21.83f,   23.12f,   24.50f,   25.96f,   27.50f,   29.14f,   30.87f

         };

    public static float getFrequency(Note note) {

        return getFrequency((int)note % frequenciesOfFirstOctaveNotes.Length, Mathf.FloorToInt((int)note / frequenciesOfFirstOctaveNotes.Length) + 1);

    }

    public static float getFrequency(NoteOnOctave note, int octave) {


        return getFrequency((int)note, octave);

    }

    public static float getFrequency(int note, int octave) {

        Debug.Log("Calculating frequency of " + note + " in octave " + octave + " result: " + (frequenciesOfFirstOctaveNotes[(int)note] * (octave + 1)));

        return frequenciesOfFirstOctaveNotes[note] * Mathf.Pow(2, (octave));

    }



}
