using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A part of a music, this is used solely because of the inspector
[System.Serializable]
public class ProceduralMusicParcelClass
{
        [SerializeReference, SerializeField]
        public ProceduralMusicParcel parcel;

        [SerializeField]
        public ProceduralMusicParcel.ParcelType parcelType;

        [SerializeField]
        //this is for the inspector, if the attributes are collapsed or not
        public bool foldout = false;

        //a class with the pure objective of storing any type of ProceduralMusicParcel and its type
        public ProceduralMusicParcelClass(ProceduralMusicParcel parcel) {
            this.parcel = parcel;
        }

        public ProceduralSound getSound(int index) {
            return parcel.getSound(index);
        }
  

}
