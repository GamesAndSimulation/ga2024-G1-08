using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindController : MonoBehaviour
{

    private GameObject player;

    [SerializeField] private float maxProximity = 2;
    [SerializeField] private float speed = 0.1f;


    [SerializeField] private SFXCyclingSoundComponent woodFoodstepsSound;
    private bool shouldPlaySteps;
    [SerializeField] private float playStepsDelay = 1f;

    protected void Awake() {
        shouldPlaySteps = true;
    }

    protected void Update() {

        Vector3 distanceVector = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z) - this.transform.position;

        if (distanceVector.sqrMagnitude > maxProximity)
            walk(distanceVector);
    }

    public void onPlayerAnnounced(Component sender, object data) {

        player = sender.gameObject;

    }


    private void walk(Vector3 distanceVector) {

        if (shouldPlaySteps) {

            woodFoodstepsSound.PlaySound();
            shouldPlaySteps = false;
            Invoke(nameof(resetShouldPlaySteps), playStepsDelay);
        }

        Vector3 toMove = Time.deltaTime * speed * distanceVector.normalized;
        this.transform.Translate(toMove, Space.World);

    }

    private void resetShouldPlaySteps() {

        shouldPlaySteps = true;

    }

}
