using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DogLevel3 : MonoBehaviour
{

    [Header("State")]
    public DogState currentState;

    [Header("State Changers")]
    private bool hasReachedTarget;

    public Transform target;
    public float targetRadius = 0.5f;

    public Transform lookTarget;
    private Vector3 defaultLookTargetOffset;
    private Vector3 lookTargetPos;

    private bool IsLookingAtTarget;

    private DogMovement moveScript;
    private DogAnimation animScript;
    private DogSounds soundScript;

    public enum DogState
    {
        idle,
        moving
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
        IsLookingAtTarget = false;
        defaultLookTargetOffset = new Vector3(0,0.7f,0.7f); //hardcoded offset because i lost my patience
    }

    void Update()
    {
        StateHandler();
        HandleLookTarget();
    }

    private void HandleLookTarget()
    {
        if (!IsLookingAtTarget)
        {
            lookTarget.parent = transform;
            lookTarget.localPosition = defaultLookTargetOffset;
        }
        else
        {
            lookTarget.parent = null;
            lookTarget.position = lookTargetPos;
        }
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

    private void LookAtTarget()
    {
        IsLookingAtTarget = true;
        lookTargetPos = target.position;
    }

    private void ClearLook()
    {
        IsLookingAtTarget = false;
    }

    public void FollowPlayer()
    {
        ClearLook();
        targetRadius = 4f;
        moveScript.navAgent.stoppingDistance = 7f;
        moveScript.navAgent.angularSpeed = 9999999999999f;
        moveScript.navAgent.speed = 2f;
        moveScript.navAgent.acceleration = 5f;

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
        LookAtTarget();
        currentState = DogState.idle;
        animScript.StartIdle();

    }

}
