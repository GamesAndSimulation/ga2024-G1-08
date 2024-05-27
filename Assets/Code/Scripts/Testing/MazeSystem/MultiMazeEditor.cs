using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MultiMazeGenerator))]
public class MultiMazeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MultiMazeGenerator multiMazeGenerator = (MultiMazeGenerator)target;

        if (GUILayout.Button("Add Mazes"))
        {
            multiMazeGenerator.AddAllMazes();
        }

        if (GUILayout.Button("Destroy All Mazes"))
        {
            multiMazeGenerator.DestroyAllMazes();
        }

        if (GUILayout.Button("Generate Mazes"))
        {
            multiMazeGenerator.GenerateAllMazes();
        }
    }
}
