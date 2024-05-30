using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "MazeTheme", order = 1)]
public class MazeTheme : ScriptableObject
{

    public string themeName;
    public Material floorMaterial;
    public Material wallMaterial;
    public Material ceilingMaterial;

    public List<GameObject> decorations;
    public List<GameObject> wallDecor;
    public List<GameObject> enemies;

    public GameObject portal;


}
