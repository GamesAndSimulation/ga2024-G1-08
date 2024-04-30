using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralMusicLoop")]
[System.Serializable]
public class ProceduralMusicLoop : ProceduralMusicParcel {


    [SerializeField]
    public List<ProceduralSound> sounds;

    public ProceduralSound getSound(int index) {
        
        if(index >= sounds.Count)
            return null;

        else
            return sounds[index];


    }
}
