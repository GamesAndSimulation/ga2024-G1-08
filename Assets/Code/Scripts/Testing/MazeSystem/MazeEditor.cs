using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MazeGenerator mazeGenerator = (MazeGenerator)target;

        if (GUILayout.Button("Create Grid"))
        {
            mazeGenerator.CreateGrid();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            mazeGenerator.DestroyGrid();
        }
        
        if (GUILayout.Button("Generate Maze"))
        {
            mazeGenerator.GenerateMaze();
        }

        if(GUILayout.Button("Generate Decor"))
        {
            mazeGenerator.GenDecorations();
        }
    }
}
