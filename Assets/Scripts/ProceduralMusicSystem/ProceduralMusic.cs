using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralMusic")]
public class ProceduralMusic : ScriptableObject
{
    [SerializeField]
    public string musicName;

    [SerializeField]
    public int beatsPerMinute;

    [SerializeField]
    public List<ProceduralSound> sounds;



}
