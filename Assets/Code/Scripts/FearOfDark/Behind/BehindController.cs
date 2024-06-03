using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindController : MonoBehaviour
{

    private GameObject player;
    private PlayerControllerFOD controllerFOD;

    private Renderer behindRenderer;

    private bool isMoving;

    [SerializeField] private float maxProximity = 2; //the proximity at which the enemy will stop getting closer to the player
    [SerializeField] private float minProximity = 10; //if the enemy is farther than the player than min proximity, it will count as if it was at this distance

    [SerializeField] private float proximityToStartMoving = 5; //the proximity at which the enemy, while still, will start moving

    [SerializeField] private float minSpeed = 0.05f; //the minimum speed the enemy has, when it is farthest from the player
    [SerializeField] private float maxSpeed = 1.0f; //the max speed the enemy has, when it is closer to the player


    [SerializeField] private SFXCyclingSoundComponent woodFoodstepsSound;
    
    private bool shouldPlaySteps;

    [SerializeField] private float maxPlayStepsDelay = 1.5f;
    [SerializeField] private float minPlayStepsDelay = 0.3f;

    protected void Awake() {
        behindRenderer = GetComponent<Renderer>();
    }

    protected void onEnabled() {

        shouldPlaySteps = true;
        isMoving = false;

    }

    protected void Update() {

        Vector3 distanceVector = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z) - this.transform.position;


        Debug.Log(behindRenderer.isVisible);
        if (!behindRenderer.isVisible) {

            if (distanceVector.magnitude >= proximityToStartMoving)
                isMoving = true;

            if (distanceVector.magnitude <= maxProximity) {
                isMoving = false;
                controllerFOD.damagePlayer(0.2f);

            } else if (isMoving)
                walk(distanceVector);

        }

    }

    public void onPlayerAnnounced(Component sender, object data) {

        player = sender.gameObject;
        controllerFOD = player.GetComponent<PlayerControllerFOD>();

    }


    private void walk(Vector3 distanceVector) {

        //if we are more distant than minProximity, we act as if we were closer
        float perceivedDistance = Mathf.Min(distanceVector.magnitude, minProximity);

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
