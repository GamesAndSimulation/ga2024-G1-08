using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class CommonPausedBehavior : MonoBehaviour {


    [SerializeField] private List<Behaviour> affectedBehaviors;

    private Rigidbody rb;
    private Animator an;
    private NavMeshAgent nagent;

    public void Awake() {

        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();
        nagent = GetComponent<NavMeshAgent>();

    }

    public void onPauseEvent(Component sender, object data) {

        bool paused = (bool)data;

        foreach (Behaviour component in affectedBehaviors)
            component.enabled = !paused;


        if (rb != null)
            rb.isKinematic = !paused;

        if(an != null) 
            an.enabled = !paused;

        if(nagent != null) 
            nagent.enabled = !paused;

    }


}