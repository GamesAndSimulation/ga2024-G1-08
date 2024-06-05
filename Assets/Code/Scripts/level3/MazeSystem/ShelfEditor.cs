using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShelfGenScript))]
public class ShelfEditor : Editor
{
   
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ShelfGenScript myScript = (ShelfGenScript)target;
        if (GUILayout.Button("Generate Shelves"))
        {
            myScript.GenerateShelves();
        }
    }
}
