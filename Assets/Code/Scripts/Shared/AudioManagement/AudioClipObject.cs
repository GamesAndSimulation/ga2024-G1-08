using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "AudioClipObject")]
public class AudioClipObject : ScriptableObject
{

    [SerializeField]
    public AudioClip clip;


    public void UpdateAssetPath() {

        Debug.Log("updating asset path");

        string thisAssetPath = AssetDatabase.GetAssetPath(this);

        if (thisAssetPath != clip.name + ".asset") {

            AssetDatabase.RenameAsset(thisAssetPath, clip.name);
            AssetDatabase.SaveAssets();


        }

    }

}



// Custom editor to listen for changes and update the file name
[CustomEditor(typeof(AudioClipObject))]
public class AudioClipObjectEditor : Editor {

    [SerializeField]
    private SerializedProperty clipProperty;

    private void OnEnable() {
        // Initialize the SerializedProperty
        clipProperty = serializedObject.FindProperty("clip");
    }

    public override void OnInspectorGUI() {


        // Update the file name if the clipProperty is changed
        serializedObject.Update();


        EditorGUI.BeginChangeCheck();
        
        base.OnInspectorGUI();

        //EditorGUILayout.PropertyField(clipProperty);

        //if the clip was changed
        if (EditorGUI.EndChangeCheck()) {

            serializedObject.Update();

            // If the string property has changed, update the file name
            AudioClipObject audioClip = (AudioClipObject)target;
            audioClip.UpdateAssetPath();



        }
        serializedObject.ApplyModifiedProperties();
    }
}