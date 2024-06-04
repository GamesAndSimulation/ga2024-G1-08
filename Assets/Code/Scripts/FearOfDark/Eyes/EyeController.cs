using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LookingAtTarget))]
public class EyeController : MonoBehaviour
{

    private LookingAtTarget lookingAtTargetComponent;

    private void Awake() {
        
        lookingAtTargetComponent = GetComponent<LookingAtTarget>();
        ChangePlayer(PlayerWatcherComponent.getPlayer());

    }

    public void setPlayer(Transform playerTransform) {

        lookingAtTargetComponent.target = playerTransform;

    }

    public void ChangePlayer(GameObject newPlayer) {

        setPlayer(newPlayer.transform);

    }


}
