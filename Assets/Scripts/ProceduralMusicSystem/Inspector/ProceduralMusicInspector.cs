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

        EditorGUILayout.LabelField("Procedural Music Definition: ");

        EditorGUILayout.Separator();

        //for each musicParcel 
        for (int i = 0; i < music.musicParcels.Count; i++) {

            

            music.musicParcels[i].foldout = EditorGUILayout.BeginFoldoutHeaderGroup(music.musicParcels[i].foldout, "Element " + i.ToString());

            if (music.musicParcels[i].foldout) {

                //Display the choice of the type of music parcel to choose
                music.musicParcels[i].parcelType = (ProceduralMusicParcel.ParcelType)EditorGUILayout.EnumPopup("Parcel Type", music.musicParcels[i].parcelType);

                //Depending on the currently chosen type
                switch (music.musicParcels[i].parcelType) {

                    case ProceduralMusicParcel.ParcelType.Loop: //in the case this is a loop

                        //if it was not a music parcel of the type loop, we create a new one that is
                        if (music.musicParcels[i].parcel != null && music.musicParcels[i].parcel.GetType() != typeof(ProceduralMusicLoop))
                            music.musicParcels[i].parcel = null;

                        //gives the option to choose the loop
                        music.musicParcels[i].parcel = (ProceduralMusicLoop)EditorGUILayout.ObjectField("Choose Loop", (ProceduralMusicLoop)music.musicParcels[i].parcel, typeof(ProceduralMusicLoop), false);
                        EditorUtility.SetDirty(target);

                        break;

                    case ProceduralMusicParcel.ParcelType.Note: //in the case this is a Note

                        //if it was not a music parcel of the type Sound or it has no value, we create a new one that is
                        if (music.musicParcels[i].parcel == null || music.musicParcels[i].parcel.GetType() != typeof(ProceduralSound))
                            music.musicParcels[i].parcel = new ProceduralSound();

                        ProceduralSound sound = (ProceduralSound)music.musicParcels[i].parcel;

                        sound.volume = EditorGUILayout.FloatField("volume", sound.volume);
                        sound.waitTime = EditorGUILayout.FloatField("waitTime", sound.waitTime);
                        sound.playTime = EditorGUILayout.FloatField("playTime", sound.playTime);
                        sound.channel = EditorGUILayout.IntField("channel", sound.channel);
                        sound.note = (NoteToFreq.Note)EditorGUILayout.EnumPopup("Note ", sound.note);


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

        //option to add a new element to the music
        if (GUILayout.Button("Add Element")) {

            // Add a new empty element to the list
            music.musicParcels.Add(new ProceduralMusicParcelClass(new ProceduralSound()));


        }

        //apply the modifications done in the inspector for the object
        serializedObject.ApplyModifiedProperties();

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