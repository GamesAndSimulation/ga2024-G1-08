using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralMusic")]
public class ProceduralMusic : ScriptableObject
{
    public string musicName;

    public List<ProceduralMusicPlayer.Sound> sounds;

}
