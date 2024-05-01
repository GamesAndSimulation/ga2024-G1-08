using System;
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



                        renderNote(music.musicParcels[i]);


                        break;

                }

                //renders the buttons that alter the element in regards to position
                if(renderMoveButtons(music, i) || renderDeleteButton(music, i))
                    break;                                                    //breaks the loop if element changes (we should not continue iterating the list after an element was deleted, moved, or other)

                EditorGUILayout.Separator();

            }

            EditorGUILayout.EndFoldoutHeaderGroup();

        }

        renderAddButton(music);


        EditorGUILayout.EndVertical();

        //apply the modifications done in the inspector for the object
        serializedObject.ApplyModifiedProperties();

    }

    private void renderNote(ProceduralMusicParcelClass parcel) {

        ProceduralSound sound = parcel.sound;


        EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Note: ", GUILayout.Width(60));
            sound.note = (NoteToFreq.NoteOnOctave)EditorGUILayout.EnumPopup("", sound.note, GUILayout.Width(120));

            EditorGUILayout.LabelField("Octave: ", GUILayout.Width(80));
            sound.octave = EditorGUILayout.IntField("", sound.octave, GUILayout.Width(120));

        EditorGUILayout.EndHorizontal();



        EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("PlayTime:");

            GUILayout.FlexibleSpace();

            sound.playTimeNumerator = EditorGUILayout.IntField("", (int)sound.playTimeNumerator, GUILayout.Width(30));
            EditorGUILayout.LabelField("/", GUILayout.Width(10));
            sound.playTimeDenominator = EditorGUILayout.IntField("", (int)sound.playTimeDenominator, GUILayout.Width(30));

            GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();



        sound.volume = EditorGUILayout.FloatField("Volume", sound.volume);


        parcel.foldoutSpecific = EditorGUILayout.Foldout(parcel.foldoutSpecific, "Specific data");

        if(parcel.foldoutSpecific)
            renderNoteSpecific(sound);


    }

    private void renderNoteSpecific(ProceduralSound sound) {

        sound.fadeoutMult = EditorGUILayout.FloatField("Fadeout", sound.fadeoutMult);

    }

    private bool renderMoveButtons(ProceduralMusic music, int i) {

        EditorGUILayout.BeginHorizontal();

        bool elementMoved;

        //the first element can only be moved down
        if (i == 0)
            elementMoved = renderMoveDownButton(music, i);

        //the last element can only be moved up
        else if(i == music.musicParcels.Count - 1)
            elementMoved = renderMoveUpButton(music, i);

        else
            elementMoved = renderMoveDownButton(music, i) || renderMoveUpButton(music, i);

        EditorGUILayout.EndHorizontal();

        return elementMoved;

    }

    private bool renderMoveUpButton(ProceduralMusic music, int i) {


        bool movedUp = false;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Up", GUILayout.Width(120))) {

            ProceduralMusicParcelClass toMoveUp = music.musicParcels[i];

            music.musicParcels[i] = music.musicParcels[i - 1];

            music.musicParcels[i - 1] = toMoveUp;

            movedUp = true;

        }

        return movedUp;


    }

    private bool renderMoveDownButton(ProceduralMusic music, int i) {

        bool movedDown = false;


        if (GUILayout.Button("Down", GUILayout.Width(120))) {

            ProceduralMusicParcelClass toMoveDown = music.musicParcels[i];

            music.musicParcels[i] = music.musicParcels[i + 1];

            music.musicParcels[i + 1] = toMoveDown;

            movedDown = true;

        }

        return movedDown;


    }

    private bool renderDeleteButton(ProceduralMusic music, int i) {

        bool valueDeleted = false;

        Rect rect = EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            //option to delete this element of the list
            if (GUILayout.Button("Delete", GUILayout.Width(120))) {

                // Add a new empty element to the list
                music.musicParcels.Remove(music.musicParcels[i]);
                valueDeleted = true;

            }

            GUILayout.FlexibleSpace();


        EditorGUILayout.EndHorizontal();

        return valueDeleted;

    }


    private void renderAddButton(ProceduralMusic music) {

        EditorGUILayout.BeginHorizontal();

        //option to add a new element to the music
        if (GUILayout.Button("Add Element")) {

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