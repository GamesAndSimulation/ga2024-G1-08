using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerChanged(GameObject newPlayer);

public class PlayerWatcherComponent : MonoBehaviour
{

    public static PlayerWatcherComponent instance { get; private set; }

    private void Awake() {

        if (instance == null)
            instance = this;


        DontDestroyOnLoad(this);

    }

    /// <summary>
    /// The current player
    /// </summary>
    public GameObject player;

    /// <summary>
    /// A csharp event that is called when the player is changed, supposed to be used by components that share their object with this PlayerWatcherComponent
    /// </summary>
    public event PlayerChanged playerChanged;

    public void addSubscriptionToPlayerChanged(PlayerChanged functionToBeCalled) {

        playerChanged += functionToBeCalled;

    }

    public GameObject getCurrentPlayer() {

        return player;

    }

    /// <summary>
    /// Added a function that'll be called (with a gameobject parameter) when the player is changed.
    /// </summary>
    /// <param name="functionToBeCalled"></param>
    public static void addSubToPlayerChanged(PlayerChanged functionToBeCalled) {

        instance.addSubscriptionToPlayerChanged(functionToBeCalled);

    }

    public static GameObject getPlayer() {

        return instance.getCurrentPlayer();

    }

    public void changePlayer(GameObject newPlayer) {

        player = newPlayer;
        playerChanged?.Invoke(newPlayer);

    }

    public void OnPlayerChanged(Component sender, object data) {

        changePlayer(sender.gameObject);

    }


}
