using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "MusicObject")]
public class MusicObject : ScriptableObject
{
    public string musicName = "unnamed";
    public List<MusicNode> musicNodes;


    [System.Serializable]
    public class MusicNode {

        public AudioClipObject clip;
        public int musicState;

        [SerializeField]
        public int passiveNextNodeIndex;

        [SerializeField]
        public int activeNextNodeIndex;

    }


}
