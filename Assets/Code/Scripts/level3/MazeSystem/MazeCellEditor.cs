using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeCell))]
public class MazeCellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MazeCell mazeCell = (MazeCell)target;

        DrawDefaultInspector();

       if (GUILayout.Button("Generate Decor"))
       {
            mazeCell.GenerateDecor();
       }

    }   

}
