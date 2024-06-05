using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A singleton other scripts can use to quickly know if the game is paused
/// </summary>
public class PausedWatcherComponent : MonoBehaviour {
    public static PausedWatcherComponent instance { get; private set; }

    private void Awake() {

        if (instance == null)
            instance = this;

        else
            Destroy(gameObject);


        DontDestroyOnLoad(this);

    }

    public static bool paused {  get { return instance.isPaused; } }

    private bool isPaused = false;


    public void OnGamePaused(Component sender, object data) {

        isPaused = (bool)data;

    }


}
