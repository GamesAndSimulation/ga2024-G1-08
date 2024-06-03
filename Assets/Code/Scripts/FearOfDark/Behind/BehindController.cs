using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindController : MonoBehaviour
{

    private GameObject player;

    [SerializeField] private float maxProximity = 2;
    [SerializeField] private float minProximity = 10;

    [SerializeField] private float minSpeed = 0.05f;
    [SerializeField] private float maxSpeed = 1.0f;


    [SerializeField] private SFXCyclingSoundComponent woodFoodstepsSound;
    
    private bool shouldPlaySteps;

    [SerializeField] private float maxPlayStepsDelay = 1.5f;
    [SerializeField] private float minPlayStepsDelay = 0.3f;

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

        //if we are more distant than minProximity, we act as if we were closer
        float perceivedDistance = Mathf.Min(distanceVector.sqrMagnitude, minProximity);

        //where we are in regards to the difference in our distance and the maxProximity
        float deltaDistance = 1 - (perceivedDistance - maxProximity) / (minProximity - maxProximity);

        if (shouldPlaySteps) {

            woodFoodstepsSound.PlaySound();
            shouldPlaySteps = false;
            Invoke(nameof(resetShouldPlaySteps), maxPlayStepsDelay - deltaDistance * (maxPlayStepsDelay - minPlayStepsDelay));
        }

        Vector3 toMove = distanceVector.normalized * Time.deltaTime * (minSpeed + (maxSpeed - minSpeed) * deltaDistance );
        this.transform.Translate(toMove, Space.World);

    }

    private void resetShouldPlaySteps() {

        shouldPlaySteps = true;

    }

}
