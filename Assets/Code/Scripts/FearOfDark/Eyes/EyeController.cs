using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LookingAtTarget))]
public class EyeController : MonoBehaviour
{

    private LookingAtTarget lookingAtTargetComponent;

    private void Awake() {
        
        lookingAtTargetComponent = GetComponent<LookingAtTarget>();

    }

    public void onPlayerAnnounced(Component sender, object data) {

        lookingAtTargetComponent.target = sender.transform;

    }



}
