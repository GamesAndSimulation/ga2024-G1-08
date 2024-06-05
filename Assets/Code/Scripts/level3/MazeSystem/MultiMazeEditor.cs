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
            EditorUtility.SetDirty(multiMazeGenerator);

        }

        if (GUILayout.Button("Destroy All Mazes"))
        {
            multiMazeGenerator.DestroyAllMazes();
            EditorUtility.SetDirty(multiMazeGenerator);

        }

        if (GUILayout.Button("Generate Mazes"))
        {
            multiMazeGenerator.GenerateAllMazes();
            EditorUtility.SetDirty(multiMazeGenerator);

        }

    }
}
