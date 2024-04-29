using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(ProceduralMusic))]
public class ProceduralMusicInspector : Editor
{
    public override VisualElement CreateInspectorGUI() {

        // Create a new VisualElement to be the root of our Inspector UI.
        VisualElement myInspector = new VisualElement();

        // Add a simple label.
        myInspector.Add(new Label("Inspector for creating music"));

        // Return the finished Inspector UI.
        return myInspector;
    }
}
