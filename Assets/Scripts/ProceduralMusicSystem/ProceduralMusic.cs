using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralMusic")]
public class ProceduralMusic : ScriptableObject
{
    public string musicName;

    [SerializeField]
    public List<ProceduralSound> sounds;



}
