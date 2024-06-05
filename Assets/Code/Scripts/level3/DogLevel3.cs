using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogLevel3 : MonoBehaviour
{

    [Header("State")]
    public DogState currentState;

    [Header("State Changers")]
    private bool hasReachedTarget;

    public Transform target;
    public float targetRadius = 0.5f;


    private DogMovement moveScript;
    private DogAnimation animScript;
    private DogSounds soundScript;

    public enum DogState
    {
        idle,
        moving,
        following
    }

    private void Awake()
    {

        moveScript = GetComponent<DogMovement>();
        animScript = GetComponent<DogAnimation>();
        soundScript = GetComponent<DogSounds>();

    }

    void Start()
    {
        currentState = DogState.idle;
        hasReachedTarget = false;
    }

    void Update()
    {
        StateHandler();
    }

    void StateHandler()
    {
        switch (currentState)
        {
            case DogState.idle:
                //Do nothing
                break;

            case DogState.moving:

                hasReachedTarget = Vector3.Distance(transform.position, target.position) < targetRadius;

                if (hasReachedTarget)
                {
                    stopMoving();
                    hasReachedTarget = false;
                }
                else
                {
                    animScript.MovingAnim(moveScript.GetSpeed());
                    moveScript.WalkToTarget(target);
                }

                break;
        }
    }

    public void setTarget(Transform newTarget)
    {

        target = newTarget;

    }

    public void playSingleBark()
    {

        animScript.PlaySingleBark();
        soundScript.PlaySingleBark();

    }

    public void goToTarget()
    {

        currentState = DogState.moving;
        hasReachedTarget = false;

    }

    public void stopMoving()
    {

        currentState = DogState.idle;
        animScript.StartIdle();

    }

}
