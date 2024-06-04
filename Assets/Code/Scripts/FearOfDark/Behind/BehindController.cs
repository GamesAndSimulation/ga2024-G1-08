using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindController : MonoBehaviour
{

    private GameObject player;
    private IsVisibleChecker isVisibleChecker;

    private bool isMoving;


    [Header("Distance Related")]
    [SerializeField] private float maxProximity = 2; //the proximity at which the enemy will stop getting closer to the player
    [SerializeField] private float minProximity = 10; //if the enemy is farther than the player than min proximity, it will count as if it was at this distance
    [SerializeField] private float deathProximity = 3; //the proximity at which the enemy will die if the player looks at it

    [SerializeField] private float proximityToStartMoving = 5; //the proximity at which the enemy, while still, will start moving

    [Header("Movement Related")]
    [SerializeField] private float minSpeed = 0.05f; //the minimum speed the enemy has, when it is farthest from the player
    [SerializeField] private float maxSpeed = 1.0f; //the max speed the enemy has, when it is closer to the player

    [SerializeField] private float delayToStartMoving = 1;

    [Header("Sound Related")]

    [SerializeField] private SFXCyclingSoundComponent woodFoodstepsSound;
    
    private bool shouldPlaySteps;

    [SerializeField] private float maxPlayStepsDelay = 1.5f;
    [SerializeField] private float minPlayStepsDelay = 0.3f;


    [Header("Damage Related")]
    [SerializeField] private GameEvent damagePlayerEvent;
    [SerializeField] private float damageOnCatchingPlayer = 0.2f;


    protected void Awake() {
        isVisibleChecker = GetComponent<IsVisibleChecker>();
        shouldPlaySteps = true;
        player = PlayerWatcherComponent.getPlayer();
    }

    protected void onEnabled() {

        shouldPlaySteps = true;
        isMoving = false;

    }

    protected void Update() {

        Vector3 distanceVector = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z) - this.transform.position;


        if (!isVisibleChecker.isVisible()) {

            if (distanceVector.magnitude >= proximityToStartMoving && !isMoving)
                Invoke(nameof(startMovingDelayed), delayToStartMoving);

            if (distanceVector.magnitude <= maxProximity) {
                isMoving = false;
                damagePlayerEvent.Raise(this, damageOnCatchingPlayer);
                destroyBehind();


            } else if (isMoving)
                walk(distanceVector);

        } else {

            if (distanceVector.magnitude <= deathProximity) {
                destroyBehind();
            }

            isMoving = false;
        }
    }

    private void destroyBehind() {

        FODEnemyManager.instance.behindWasKilled();
        Destroy(gameObject);
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

    private void startMovingDelayed() {

        isMoving = true;

    }

}
