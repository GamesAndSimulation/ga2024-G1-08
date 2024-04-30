using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A part of a music
public interface ProceduralMusicParcel
{

    public enum ParcelType { Loop, Note}; 


    public ProceduralSound getSound(int index);

}
