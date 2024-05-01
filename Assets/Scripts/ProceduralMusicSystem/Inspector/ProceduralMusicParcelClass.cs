using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

//A part of a music, this is used solely because of the inspector
[System.Serializable]
public class ProceduralMusicParcelClass
{

    //the types there can be of parts of music. A part of a music can be a note (sound) or a loop
    public enum ParcelType { Loop, Note };



    [SerializeField]
    public ProceduralMusicLoop loop; //this is only used if it is a loop

    [SerializeField]
    public ProceduralSound sound; //this is only used if it is a sound




    [SerializeField]
    public ParcelType parcelType; //the type of this parcel



    public bool foldout = true; //this is for the inspector, if the attributes are collapsed or not
    public bool foldoutSpecific = true;




    public ProceduralMusicParcelClass(ProceduralSound sound) {
        this.sound = sound;
        this.loop = null;
        this.parcelType = ParcelType.Note;
    }
    public ProceduralMusicParcelClass(ProceduralMusicLoop loop) {
        this.sound = null;
        this.loop = loop;
        this.parcelType = ParcelType.Loop;
    }


    public void changeTypeTo(ParcelType parcelType) {

        if (parcelType != this.parcelType) {

            switch (parcelType) {

                case ParcelType.Loop:

                    loop = null;
                    sound = null;
                    break;


                case ParcelType.Note:

                    loop = null;
                    sound = new ProceduralSound();
                    break;

            }

            this.parcelType = parcelType;
        }

    }


    public ProceduralSound getSound(int index) {
        
        switch(parcelType) { 
            
            case ParcelType.Loop:
                
                return loop.getSound(index);
                //break;

            case ParcelType.Note:

                return sound.getSound(index);
                //break;


        }

        return null;

    }
  

}
