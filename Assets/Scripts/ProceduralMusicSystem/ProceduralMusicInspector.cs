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
    private SerializedProperty musicName;
    private SerializedProperty beatsPerMinute;

    void OnEnable() {

        soundList = serializedObject.FindProperty("sounds");
        musicName = serializedObject.FindProperty("musicName");
        beatsPerMinute = serializedObject.FindProperty("beatsPerMinute");
    }


    public override void OnInspectorGUI() {

        serializedObject.Update();

        EditorGUILayout.PropertyField(musicName);
        EditorGUILayout.PropertyField(beatsPerMinute);
        EditorGUILayout.PropertyField(soundList);


        serializedObject.ApplyModifiedProperties();
    }

}
#endif