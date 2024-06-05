using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CommonPausedBehavior : MonoBehaviour {


    [SerializeField] private List<Behaviour> affectedBehaviors;

    private Rigidbody rb;
    private Animator an;

    public void Awake() {

        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();

    }

    public void onPauseEvent(Component sender, object data) {

        bool paused = (bool)data;

        foreach (Behaviour component in affectedBehaviors)
            component.enabled = !paused;


        if (rb != null)
            rb.isKinematic = !paused;

        if(an != null) 
            an.enabled = !paused;

    }


}