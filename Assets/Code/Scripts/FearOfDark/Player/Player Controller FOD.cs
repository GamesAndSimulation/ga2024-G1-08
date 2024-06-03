using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class PlayerControllerFOD : MonoBehaviour {




    [Header("State")]

    [Range(0, 1)] public float damage;

    [Range(0, 1)] public float movementMultiplier;


    [Header("Sounds")]

    [SerializeField] protected FODPlayerSound heartSoundController = new FODPlayerSound(null, new Vector2(0.85f, 1.5f), new Vector2(0.01f, 0.2f));
    [SerializeField] protected FODPlayerSound breathingSoundController = new FODPlayerSound(null, new Vector2(0.85f, 1.0f), new Vector2(0.05f, 0.25f));


    protected void Start() {

        setupPlayerSounds();


    }

    public void setupPlayerSounds() {

        heartSoundController.setup();
        breathingSoundController.setup();

    }

    // Update is called once per frame
    protected void Update() {

        heartSoundController.updateSound(damage, damage);
        heartSoundController.updateSound(damage, damage);

    }

}
