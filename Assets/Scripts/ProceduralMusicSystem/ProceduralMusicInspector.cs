using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

//this if means that this code is only compiled if there is an UnityEditor around (not compiled for players)
#if UNITY_EDITOR
[CustomEditor(typeof(ProceduralMusic))]
public class ProceduralMusicInspector : Editor
{


    private SerializedProperty soundList;

    void OnEnable() {
        soundList = serializedObject.FindProperty("sounds");
    }


    public override void OnInspectorGUI() {

        serializedObject.Update();

        EditorGUILayout.PropertyField(soundList);

        serializedObject.ApplyModifiedProperties();
    }

}
#endif