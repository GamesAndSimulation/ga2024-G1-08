using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

//this if means that this code is only compiled if there is an UnityEditor around (not compiled for players)
#if UNITY_EDITOR
[CustomEditor(typeof(ProceduralMusic))] //this means this inspector is used for ProceduralMusic
public class ProceduralMusicInspector : Editor
{


    public override void OnInspectorGUI() {

        serializedObject.Update();

        DrawDefaultInspectorExceptSerializedProperty("musicParcels");

        ProceduralMusic music = (ProceduralMusic)target; //the target is the object we're editing, and we know it is a Procedural Music

        

        if (music.musicParcels == null) { 
            music.musicParcels = new List<ProceduralMusicParcelClass>();
            Debug.Log("Music parcels is null at the begining");
        }

        EditorGUILayout.Separator();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Procedural Music Definition: ");

        EditorGUILayout.Separator();

        //for each musicParcel 
        for (int i = 0; i < music.musicParcels.Count; i++) {

            music.musicParcels[i].foldout = EditorGUILayout.BeginFoldoutHeaderGroup(music.musicParcels[i].foldout, "Element " + i.ToString());

            if (music.musicParcels[i].foldout) {

                //Display the choice of the type of music parcel to choose
                music.musicParcels[i].changeTypeTo ( (ProceduralMusicParcelClass.ParcelType)EditorGUILayout.EnumPopup("Parcel Type", music.musicParcels[i].parcelType));

                //Depending on the currently chosen type
                switch (music.musicParcels[i].parcelType) {

                    case ProceduralMusicParcelClass.ParcelType.Loop: //in the case this is a loop

                        //gives the option to choose the loop
                        music.musicParcels[i].loop = (ProceduralMusicLoop)EditorGUILayout.ObjectField("Choose Loop", (ProceduralMusicLoop)music.musicParcels[i].loop, typeof(ProceduralMusicLoop), false);
                        EditorUtility.SetDirty(target);

                        break;

                    case ProceduralMusicParcelClass.ParcelType.Note: //in the case this is a Note


                        ProceduralSound sound = music.musicParcels[i].sound;

                        renderNote(sound);


                        break;

                }

                //option to delete this element of the list
                if (GUILayout.Button("Delete")) {

                    // Add a new empty element to the list
                    music.musicParcels.Remove(music.musicParcels[i]);

                    break; //because we altered the list, it is best to leave the loop

                    

                }

                EditorGUILayout.Separator();

            }

            EditorGUILayout.EndFoldoutHeaderGroup();

        }

        renderAddButton(music);

        EditorGUILayout.EndVertical();

        //apply the modifications done in the inspector for the object
        serializedObject.ApplyModifiedProperties();

    }

    private void renderNote(ProceduralSound sound) {


        EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Note: ", GUILayout.Width(60));
            sound.note = (NoteToFreq.NoteOnOctave)EditorGUILayout.EnumPopup("", sound.note, GUILayout.Width(120));

            EditorGUILayout.LabelField("Octave: ", GUILayout.Width(80));
            sound.octave = EditorGUILayout.IntField("", sound.octave, GUILayout.Width(120));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("PlayTime:");

            sound.playTimeNumerator = EditorGUILayout.IntField("", sound.playTimeNumerator, GUILayout.Width(30));
            EditorGUILayout.LabelField("/", GUILayout.Width(10));
            sound.playTimeDenumerator = EditorGUILayout.IntField("", sound.playTimeDenumerator, GUILayout.Width(30));

        EditorGUILayout.EndHorizontal();

        sound.volume = EditorGUILayout.FloatField("volume", sound.volume);


    }



    private void renderBringDown(ProceduralMusic music, int index) {

        EditorGUILayout.BeginHorizontal();

        //option to add a new element to the music
        if (GUILayout.Button("Add Element", GUILayout.Width(100))) {

            // Add a new empty element to the list
            music.musicParcels.Add(new ProceduralMusicParcelClass(new ProceduralSound()));


        }

        EditorGUILayout.EndHorizontal();

    }

    private void renderAddButton(ProceduralMusic music) {

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        //option to add a new element to the music
        if (GUILayout.Button("Add Element", GUILayout.Width(100))) {

            // Add a new empty element to the list
            music.musicParcels.Add(new ProceduralMusicParcelClass(new ProceduralSound()));


        }

        EditorGUILayout.EndHorizontal();

    }

    // Method to draw the default inspector while excluding a specific property
    private void DrawDefaultInspectorExceptSerializedProperty(string propertyName) {


        // Iterate through all properties and draw them except the one to hide
        SerializedProperty iterator = serializedObject.GetIterator();

        bool enterChildren = true;

        while (iterator.NextVisible(enterChildren)) {

            enterChildren = false;

            if (iterator.name != propertyName) {
                EditorGUILayout.PropertyField(iterator, true);
            }
        }

    }

}
#endif