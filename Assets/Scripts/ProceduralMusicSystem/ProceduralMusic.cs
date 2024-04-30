using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralMusic")]
[System.Serializable]
public class ProceduralMusic : ScriptableObject
{
    [SerializeField]
    public string musicName;

    [SerializeField]
    public int beatsPerMinute;

    [SerializeField]
    public int noteIntervalDelayMili; //additional delay between notes in miliseconds

    [SerializeField]
    public List<ProceduralMusicParcelClass> musicParcels;



}
